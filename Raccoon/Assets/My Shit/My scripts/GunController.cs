using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.1f;
    public int clipSize = 32;
    public int reservedAmmoCapacity = 270;
    public float reloadCounter = 2.5f;

    public float damage = 10f;
    public float range = 100f;
    private Camera cam;
    public GameObject impactEffect;
    private int playerLayer = 9;
    public Text ammoUI, reservedAmmo;
    public KeyCode reloadButton;
    public bool canReload = true;

    //Variables that change throughout the code
    bool canShoot;
    public int CurrentAmmoInClip = 32;
    public int ammoInReserve = 200;

    //Muzzle flash
    public Image muzzleflashimage;
    public Sprite[] flashes;
    //Aiming
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;
    public float aimSmoothing = 10;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 1;
    Vector2 currentRotation;
    public float weaponSwayAmount = 10;
    public Transform playerBody;

    float xRotation = 0f;
    //Weapon Recoil
    public bool randomizeRecoil;
    public Vector2 randomRecoilConstraints;
    //You only need to assign this if randomizerRecoil is off
    public Vector2[] recoilPattern;
    Animator animator;

    private void Start()
    {
        canShoot = true;
        cam = GetComponentInParent<Camera>();
        playerBody = GetComponentInParent<Transform>();
        animator = GetComponent<Animator>();

    }
    private void Update()
    {
        DetermineAim();
        DetermineRotation();
        if (Input.GetMouseButton(0) && canShoot && CurrentAmmoInClip > 0)
        {
            canReload = false;
            canShoot = false;
            CurrentAmmoInClip--;
            StartCoroutine(ShootGun());
        }
        if (Input.GetKeyDown(reloadButton) && canReload == true)
        {
            StartCoroutine(Reload());
        }
        ammoUI.GetComponent<Text>().text = CurrentAmmoInClip.ToString();
        reservedAmmo.GetComponent<Text>().text = ammoInReserve.ToString();
    }
    void DetermineRecoil()
    {
        transform.localPosition -= Vector3.forward * 0.1f;

        if (randomizeRecoil)
        {
            float xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
            float yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);

            Vector2 recoil = new Vector2(xRecoil, yRecoil);
            currentRotation += recoil;
        }
        else
        {
            int currentStep = clipSize + 1 - CurrentAmmoInClip;
            currentStep = Mathf.Clamp(currentStep, 0, recoilPattern.Length - 1);
            currentRotation += recoilPattern[currentStep];
        }
    }
    IEnumerator ShootGun()
    {
        DetermineRecoil();
        StartCoroutine(MuzzleFlash());
        RayCastForEnemy();
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
        canReload = true;
    }
    IEnumerator MuzzleFlash()
    {
        muzzleflashimage.sprite = flashes[Random.Range(0, flashes.Length)];
        muzzleflashimage.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        muzzleflashimage.sprite = null;
        muzzleflashimage.color = new Color(0, 0, 0, 0);
    }
    void RayCastForEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, playerLayer))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
    void DetermineRotation()
    {
        Vector2 mouseAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * MouseX);
        transform.localPosition += (Vector3)mouseAxis * weaponSwayAmount / 1000;

    }
    void DetermineAim()
    {
        Vector3 target = normalLocalPosition;
        if (Input.GetMouseButton(1)) target = aimingLocalPosition;
        Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);

        transform.localPosition = desiredPosition;
    }
    IEnumerator Reload()
    {
        if (CurrentAmmoInClip < clipSize && ammoInReserve > 0)
        {
            int amountNeeded = clipSize - CurrentAmmoInClip;
            if (amountNeeded >= ammoInReserve)
            {
                Debug.Log("running");
                animator.SetBool("IsReloading", true);
                canShoot = false;
                yield return new WaitForSeconds(reloadCounter);
                animator.SetBool("IsReloading", false);
                CurrentAmmoInClip += ammoInReserve;
                ammoInReserve -= amountNeeded;
                canShoot = true;

            }
            else
            {
                Debug.Log("slut");
                animator.SetBool("IsReloading", true);
                canShoot = false;
                yield return new WaitForSeconds(reloadCounter);
                animator.SetBool("IsReloading", false);
                CurrentAmmoInClip = clipSize;
                ammoInReserve -= amountNeeded;
                canShoot = true;

            }
        }
    }
    public void SaveInfo() 
    {
        Info info = new Info(this);
        string json = JsonUtility.ToJson(info);

        File.WriteAllText(Application.persistentDataPath + "/gun", json);
        Debug.Log(File.ReadAllText(Application.persistentDataPath + "/gun"));
    }
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gun"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/gun");
            Info info = JsonUtility.FromJson<Info>(json);
            CurrentAmmoInClip = info.currentammo;
            ammoInReserve = info.ammoinreserves;
            Debug.Log(json);
        }
        else
            Debug.Log("File Doesnt Exists Yet");
        
    }

    public void DeleteFile()
    {
        if (File.Exists(Application.persistentDataPath + "/gun"))
        {
            File.Delete(Application.persistentDataPath + "/gun");
            Debug.Log("file Deleted");
            ammoInReserve = reservedAmmoCapacity;
            CurrentAmmoInClip = clipSize;
        }
        else
            Debug.Log("t");
        ammoInReserve = reservedAmmoCapacity;
        CurrentAmmoInClip = clipSize;
        return;
    }
}   



[System.Serializable]
public class Info
{
    public int currentammo, ammoinreserves;

    public Info(GunController gun)
    {
        currentammo = gun.CurrentAmmoInClip;
        ammoinreserves = gun.ammoInReserve;
    }
}