using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public NavMeshAgent controller;
    public MeshRenderer mesh;
    public EnemyShooterScript shooterScript;
    public Transform player;

    public Transform View;

    public float lookSpeed = 10f;
    public float shootTimer = 0.5f;
    public float shootTimerCounter = 0f;

    public bool reloading = false;

    public ParticleSystem deathSystem;
    public AudioSource deathSound;

    public int type;
    bool dead = false;

    public float health = 50;
    public float timeToDie = 1f;
    public float timeCounter = 0f;

    //AI
    public bool lockedOn = false;
    //public float watchDistance = 10f;


    void Update()
    {
        int timeNum = SlowTime.timeNumber;
        if (timeCounter > 0)
        {
            timeCounter -= Time.deltaTime;
        }
        else if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (health > 0)
        {
            
            //AI
            Ray ray = new Ray(transform.position, player.position - transform.position);
            RaycastHit rayOut;
            if (!lockedOn)
            {
                if (Physics.Raycast(ray, out rayOut, 10))
                {
                    if (rayOut.transform.CompareTag("Player"))
                    {
                        lockedOn = true;
                    }
                }
            }
            else
            {
                controller.SetDestination(player.position);

                if (timeNum == 0)
                {
                    controller.isStopped = true;
                }
                else
                {
                    controller.isStopped = false;
                }

                /*if (Vector3.Distance(transform.position, player.position) > watchDistance)
                {
                    controller.isStopped = false;
                    controller.SetDestination(player.position);
                }
                else
                {
                    controller.isStopped = true;
                }*/

            }

            //AIM

            Vector3 dir = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(View.rotation, lookRotation, Time.deltaTime * lookSpeed * timeNum).eulerAngles;
            View.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            //SHOOT

            if (Physics.Raycast(ray, out rayOut, 10))
            {
                if (rayOut.transform.CompareTag("Player"))
                {
                    if (shooterScript.currentAmmo == 0 && !reloading)
                    {
                        shooterScript.reloadKey = true;
                        reloading = true;
                    }
                    else if (!reloading)
                    {
                        if (shootTimerCounter > 0)
                        {
                            shootTimerCounter -= Time.deltaTime * timeNum;

                        }
                        else
                        {
                            shootTimerCounter = shootTimer;
                            shooterScript.shootKey = true;
                        }
                    }  
                }
            }
        }
    }

    public void Damage(float num)
    {
        health -= num;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    void Die()
    {
        if (!dead)
        {
            shooterScript.enabled = false;
            mesh.enabled = false;
            deathSystem.Play();
            deathSound.Play();
            shooterScript.DropGun();
            timeCounter = timeToDie;
        }
        dead = true;
    }

}
