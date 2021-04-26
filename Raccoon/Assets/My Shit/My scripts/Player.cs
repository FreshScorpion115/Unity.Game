using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera playerCamera;
    public GameObject bulletPrefab1;
    public GameObject bulletPrefab2;
    public GameObject bulletPoint;
    public Quaternion playerRotation { get => playerCamera.transform.rotation; set => playerRotation = playerRotation; }

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bulletObject = Instantiate(bulletPrefab1, bulletPoint.transform.position, playerCamera.transform.rotation);
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameObject bulletObject = Instantiate(bulletPrefab2, bulletPoint.transform.position, playerRotation);
            
        }
    }
   
}
