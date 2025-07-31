using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator lightanimator;
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] int weaponDamage = 10;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitParticle;
    
    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    [SerializeField] float timeBetweenShots = 0.5f;

    [SerializeField] AudioClip gunShot;
    [SerializeField] AudioClip emptyGun;

    [SerializeField] TextMeshProUGUI ammoText;

    bool canShoot = true;

    AudioSource audioPlayer;

    private void OnEnable()
    {
        audioPlayer = GetComponent<AudioSource>();
        canShoot = true;
    }

    void Update()
    {
        DisplayAmmo();

        if (Input.GetMouseButtonDown(0) && canShoot == true)
        {
            canShoot = false;
            StartCoroutine(Shoout());
        }
    }

    private void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = ammoType + " : " + currentAmmo.ToString();
    }

    IEnumerator Shoout()
    {
        if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            audioPlayer.clip = gunShot;
            animator.SetTrigger("GunFire");
            lightanimator.SetTrigger("Fire");
            audioPlayer.Play();
            PlayMuzzleFlash();
            ProccessRaycast();
            ammoSlot.ReduceCurrentAmmo(ammoType);
        }
        else
        {
            audioPlayer.clip = emptyGun;
            audioPlayer.Play();
        }

        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }


    private void PlayMuzzleFlash()
    {
        
        muzzleFlash.Play();
    }


    private void ProccessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
        {
            CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target == null) { return; }
            target.TakeDamage(weaponDamage);
        }
        else
        {
            return;
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
       GameObject impact = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, .5f);
    }
}
