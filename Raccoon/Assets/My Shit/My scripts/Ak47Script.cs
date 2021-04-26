using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ak47Script : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    private Camera cam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    private int playerLayer = 9;
    private bool canShoot = true;

    private void Start()
    {
        cam = GetComponentInParent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(canShoot == true)
            {
                Shoot();
            }
            
        }
    }
    void Shoot()
    {
        muzzleFlash.Play();
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
}
