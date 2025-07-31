using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThowCocktail : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject cocktailToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;

    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;

    [SerializeField] GameObject ammoTextObj;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] MeshRenderer mesh;

    void Start()
    {
        readyToThrow = true;
        ShowMesh();
    }



    void Update()
    {
        DisplayAmmo();
        ShowMesh();
        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToThrow)
        {
            Throw();
        }
    }

    private void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = ammoType + " : " + currentAmmo.ToString();
    }

    private void Throw()
    {
        if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            readyToThrow = false;
            GameObject projectile = Instantiate(cocktailToThrow, attackPoint.position, cam.rotation);

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            Vector3 forceDirection = cam.transform.forward;

            RaycastHit hit;

            if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
            {
                forceDirection = (hit.point - attackPoint.position).normalized;
            }

            Vector3 addForce = forceDirection * throwForce + transform.up * throwUpwardForce;

            projectileRb.AddForce(addForce, ForceMode.Impulse);

            totalThrows--;

            Invoke(nameof(ResetThrow), throwCooldown);

            ammoSlot.ReduceCurrentAmmo(ammoType);
        }
    }

    private void ShowMesh()
    {
        if (ammoSlot.GetCurrentAmmo(ammoType) == 0)
        {
            mesh.enabled = false;
        }
        else if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            ammoTextObj.SetActive(true);
            mesh.enabled = true;
        }
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
