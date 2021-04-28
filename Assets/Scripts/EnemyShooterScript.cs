using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyShooterScript : MonoBehaviour
{
    public Transform Camera;
    public EnemyScript enemyScript;

    //animation
    public Animator pistolAnimator;
    public Animator rifleAnimator;
    public Animator gpistolAnimator;

    //particles
    public ParticleSystem fireParticles;
    public Vector3 pistolParticlePos;
    public Vector3 rifleParticlePos;

    //gun models
    public GameObject pistolModel;
    public GameObject rifleModel;
    public GameObject gpistolModel;


    //bullets
    public GameObject bullet;
    public Transform bulletSrc;

    //dropped guns
    public GameObject droppedPistol;
    public GameObject droppedRifle;
    public GameObject droppedGPistol;

    //audio
    public AudioSource gunShot;
    public AudioSource tinkSound;

    //actions
    public bool reloadKey;
    public bool pickupKey;
    public bool dropKey;
    public bool ultKey;
    public bool shootKey;

    //gun cd
    float cd = 0.3f;
    float cdCounter = 0f;
    bool doneCd = false;

    //ammo
    public int currentAmmo = 10;
    public int maxAmmo = 10;
    public int reserveAmmo = 30;
    bool doneReloading = false;
    public float reloadCounter = 0f;
    float reloadTime = 1.1f;
    bool reloadTriggered = false;

    //weapon handling
    public int currentGun = 1;

    private void Update()
    {
        int timeNum = SlowTime.timeNumber;
        if (cdCounter > 0)
        {
            doneCd = false;
            cdCounter -= Time.deltaTime * timeNum;
        }
        else if (doneCd == false)
        {
            doneCd = true;
            cdCounter = 0;
        }

        //reload
        if (reloadCounter > 0)
        {
            if (reloadTriggered == false)
            {
                reloadTriggered = true;
                if (currentGun == 1)
                {
                    pistolAnimator.SetTrigger("Reload");
                }
                else if (currentGun == 2)
                {
                    rifleAnimator.SetTrigger("Reload");
                }
                else if (currentGun == 3)
                {
                    gpistolAnimator.SetTrigger("Reload");
                }
                doneReloading = false;
            }
            reloadCounter -= Time.deltaTime * timeNum;
        }
        else if (doneReloading == false)
        {
            reloadCounter = 0f;
            doneReloading = true;
            if (reserveAmmo >= (maxAmmo - currentAmmo))
            {
                reserveAmmo -= (maxAmmo - currentAmmo);
                currentAmmo = maxAmmo;
            }
            else
            {
                currentAmmo = reserveAmmo;
                reserveAmmo = 0;
            }
            reloadTriggered = false;
            enemyScript.reloading = false;
        }

        //inputs
        if (shootKey && reloadCounter == 0 && cdCounter == 0 && bullet != null)
        {
            shootKey = false;
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else if (currentGun != 0)
            {
                tinkSound.Play();
                cdCounter = cd;
            }
        }

        if (reloadKey && reloadCounter == 0 && reserveAmmo > 0 && currentAmmo != maxAmmo)
        {
            reloadKey = false;
            reloadCounter = reloadTime;
        }

        if (dropKey)
        {
            dropKey = false;
            DropGun();
        }
    }

    private void Shoot()
    {
        cdCounter = cd;
        Instantiate(bullet, bulletSrc.position, bulletSrc.rotation);
        gunShot.Play();
        if (currentGun == 1)
        {
            pistolAnimator.SetTrigger("Shoot");
        }
        else if (currentGun == 2)
        {
            rifleAnimator.SetTrigger("Shoot");
        }
        else if (currentGun == 3)
        {
            gpistolAnimator.SetTrigger("Shoot");
        }

        fireParticles.Play();

        currentAmmo--;
    }

    public void DropGun()
    {
        switch (currentGun)
        {
            case 0:
                break;
            case 1:
                pistolModel.SetActive(false);

                GameObject droppedP = Instantiate(droppedPistol, Camera.position + Camera.forward, Camera.rotation);
                GroundGunScript ggsP = droppedP.GetComponent<GroundGunScript>();
                if (ggsP != null)
                {
                    ggsP.currentAmmo = currentAmmo;
                    ggsP.reserveAmmo = reserveAmmo;
                }
                currentAmmo = 0;
                reserveAmmo = 0;
                currentGun = 0;

                break;
            case 2:
                rifleModel.SetActive(false);

                GameObject droppedR = Instantiate(droppedRifle, Camera.position + Camera.forward, Camera.rotation);
                GroundGunScript ggsR = droppedR.GetComponent<GroundGunScript>();
                if (ggsR != null)
                {
                    ggsR.currentAmmo = currentAmmo;
                    ggsR.reserveAmmo = reserveAmmo;
                }
                currentAmmo = 0;
                reserveAmmo = 0;
                currentGun = 0;

                break;
            case 3:
                gpistolModel.SetActive(false);

                GameObject droppedgP = Instantiate(droppedGPistol, Camera.position + Camera.forward, Camera.rotation);
                GroundGunScript ggsgP = droppedgP.GetComponent<GroundGunScript>();
                if (ggsgP != null)
                {
                    ggsgP.currentAmmo = currentAmmo;
                    ggsgP.reserveAmmo = reserveAmmo;
                }
                currentAmmo = 0;
                reserveAmmo = 0;
                currentGun = 0;

                break;
            default:
                break;
        }
    }

}