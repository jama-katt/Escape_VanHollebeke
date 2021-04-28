using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 5;
    public float speed = 20;
    public LayerMask bulletHoleMask;

    public GameObject bulletHole;

    float killTimer = 10f;
    public Rigidbody body;

    void Start()
    {
        body.velocity = gameObject.transform.forward * speed;
    }

    private void Update()
    {
        int timeNum = SlowTime.timeNumber;
        if (killTimer > 0)
        {
            killTimer -= Time.deltaTime * timeNum;
        }
        else
        {
            Destroy(gameObject);
        }
        
        if (timeNum == 0)
        {
            body.velocity = Vector3.zero;
        }
        else
        {
            body.velocity = gameObject.transform.forward * speed;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (!other.transform.CompareTag("gunModel") && !other.transform.CompareTag("Bullet"))
        {
            if (other.transform.CompareTag("Player"))
            {
                PlayerScript temp = other.GetComponent<PlayerScript>();
                if (temp != null)
                {
                    temp.Damage(damage);
                }
            }
            else if (other.transform.CompareTag("Enemy"))
            {
                EnemyScript temp = other.GetComponent<EnemyScript>();
                if (temp != null)
                {
                    temp.Damage(damage);
                }
            }
            else
            {
                body.velocity = Vector3.zero;
                Ray ray = new Ray(transform.position -  transform.forward, transform.forward);
                RaycastHit rayOut;
                if (Physics.Raycast(ray, out rayOut, 2, bulletHoleMask))
                {
                    //Debug.Log(rayOut.transform.gameObject.name);
                    Instantiate(bulletHole, rayOut.point, Quaternion.LookRotation(rayOut.normal));
                }
            }
            Destroy(gameObject);
        }   
    }
}
