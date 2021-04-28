using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShooterScript : MonoBehaviour
{
    public Transform Camera;

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
    GameObject bullet;
    public GameObject bullet1;
    public GameObject bullet2;
    public Transform bulletSrc;

    //dropped guns
    public LayerMask groundGunMask;
    public GameObject droppedPistol;
    public GameObject droppedRifle;
    public GameObject droppedGPistol;

    //ui
    public TMP_Text currentAmmoText;
    public TMP_Text reserveAmmoText;
    public TMP_Text reloadingText;
    public TMP_Text ultText;
    public TMP_Text ultDurText;
    public GameObject overlay;

    //audio
    public AudioSource gunShot;
    public AudioSource tinkSound;
    public AudioSource ultSound;

    //keys
    public KeyCode reloadKey;
    public KeyCode pickupKey;
    public KeyCode dropKey;
    public KeyCode ultKey;

    //gun cd
    float cd = 0.1f;
    float cdCounter = 0f;
    bool doneCd = false;

    //ammo
    int currentAmmo = 0;
    int maxAmmo = 0;
    int reserveAmmo = 0;
    bool doneReloading = false;
    float reloadCounter = 0f;
    float reloadTime = 1.1f;
    bool reloadTriggered = false;

    //weapon handling
    int currentGun = 0;

    //ult
    float ultTime = 30f;
    float ultCounter = 0f;
    bool ultReady = false;
    float ultDur = 5f;
    float ultDurCounter = 0f;

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

        if (ultCounter > 0)
        {
            ultCounter -= Time.deltaTime * timeNum;
            ultText.text = "" + ((int)ultCounter + 1);
        }
        else
        {
            ultReady = true;
            ultCounter = 0;
            ultText.text = "ready";
        }

        if (ultDurCounter > 0)
        {
            ultDurCounter -= Time.deltaTime;
            ultDurText.text = ultDurCounter.ToString("F2");
        }
        else
        {
            ultDurCounter = 0;
            ultDurText.text = "";
            StopUlt();
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
                reloadingText.text = "reloading...";
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
            AmmoToScreen();
            reloadingText.text = "";
            reloadTriggered = false;
        }

        //inputs
        if (Input.GetButton("Fire1") && reloadCounter == 0 && cdCounter == 0 && bullet != null)
        {
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

        if (Input.GetKeyDown(reloadKey) && reloadCounter == 0 && reserveAmmo > 0)
        {
            reloadCounter = reloadTime;
        }

        if (Input.GetKeyDown(dropKey))
        {
            DropGun();
        }

        if (Input.GetKeyDown(pickupKey))
        {
            if (currentGun == 0)
            {
                Ray ray = new Ray(Camera.position, Camera.forward);
                RaycastHit rayOut;
                if (Physics.Raycast(ray, out rayOut, 3, groundGunMask))
                {
                    GetGun(rayOut.transform.gameObject);
                    Debug.Log("gun cast successful");
                }
                else
                    Debug.Log("gun cast failed");
            }
        }

        if (Input.GetKeyDown(ultKey) && ultReady)
        {
            Ult();
        }
    }

    private void AmmoToScreen()
    {
        currentAmmoText.text = "" + currentAmmo;
        reserveAmmoText.text = "" + reserveAmmo;

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
        AmmoToScreen();
    }

    private void DropGun()
    {
        switch (currentGun)
        {
            case 0:
                break;
            case 1:
                pistolAnimator.CrossFade("Idle", 0f);
                pistolAnimator.Update(0f);
                pistolAnimator.Update(0f);

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

                AmmoToScreen();
                reloadingText.text = "no gun";
                break;
            case 2:
                rifleAnimator.CrossFade("Idle", 0f);
                rifleAnimator.Update(0f);
                rifleAnimator.Update(0f);

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

                AmmoToScreen();
                reloadingText.text = "no gun";
                break;
            case 3:
                pistolAnimator.CrossFade("Idle", 0f);
                pistolAnimator.Update(0f);
                pistolAnimator.Update(0f);

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

                AmmoToScreen();
                reloadingText.text = "no gun";
                break;
            default:
                break;
        }
    }

    private void GetGun(GameObject gun)
    {
        GroundGunScript ggs = gun.GetComponent<GroundGunScript>();
        if (ggs != null)
        {
            currentAmmo = ggs.currentAmmo;
            reserveAmmo = ggs.reserveAmmo;
            currentGun = ggs.gunType;
        }

        switch (currentGun)
        {
            case 0:
                break;
            case 1:
                pistolModel.SetActive(true);

                cd = 0.3f;
                maxAmmo = 10;
                fireParticles.transform.localPosition = pistolParticlePos;
                gunShot.pitch = 1.5f;
                bullet = bullet1;

                AmmoToScreen();
                reloadingText.text = "";
                Destroy(gun);
                break;
            case 2:
                rifleModel.SetActive(true);

                cd = 0.1f;
                maxAmmo = 30;
                fireParticles.transform.localPosition = rifleParticlePos;
                gunShot.pitch = 1f;
                bullet = bullet1;

                AmmoToScreen();
                reloadingText.text = "";
                Destroy(gun);
                break;
            case 3:
                gpistolModel.SetActive(true);

                cd = 1f;
                maxAmmo = 6;
                fireParticles.transform.localPosition = pistolParticlePos;
                gunShot.pitch = 3f;
                bullet = bullet2;

                AmmoToScreen();
                reloadingText.text = "";
                Destroy(gun);
                break;
            default:
                break;
        }
    }

    private void Ult()
    {
        overlay.SetActive(true);
        ultReady = false;
        ultCounter = ultTime;
        ultSound.Play();
        SlowTime.timeNumber = 0;
        //Debug.Log(SlowTime.timeNumber);
        ultDurCounter = ultDur;
    }

    private void StopUlt()
    {
        overlay.SetActive(false);
        ultSound.Stop();
        SlowTime.timeNumber = 1;
    }
}