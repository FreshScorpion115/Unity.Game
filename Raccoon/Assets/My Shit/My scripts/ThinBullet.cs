using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinBullet : MonoBehaviour
{
    public float speed = 15f;
    public float bulletLifeTime = 2f;

    private float lifeTimer;
    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = bulletLifeTime;
    }

    // Update is called once per frame
    void Update()
    // makes the bullet move
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        bulletLifeTime -= Time.deltaTime;
        if (bulletLifeTime <= 0)
        {
            Destroy(this.gameObject);
        }

    }
    void OnCollisionEnter(UnityEngine.Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        } else if (collisionInfo.gameObject.tag == "Killable")
        {
            Destroy(collisionInfo.gameObject);
            Destroy(gameObject);
        }
    }
}
