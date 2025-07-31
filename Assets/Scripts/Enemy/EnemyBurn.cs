using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBurn : MonoBehaviour
{
    [SerializeField] GameObject core;
    [SerializeField] GameObject fire;
    [SerializeField] GameObject parent;
    Rigidbody coreRigid;
    CorePickUp corePickUp;
    Collider coreCollider;
    Collider parentCollider;
    EnemyHealth health;
    private ParticleSystem burnParticles;
    public bool isBurning = false;
    public bool isBurned = false;

    void Start()
    {
        burnParticles = fire.GetComponent<ParticleSystem>();
        fire.SetActive(false);
        coreRigid = core.GetComponent<Rigidbody>();
        corePickUp = core.GetComponent<CorePickUp>();
        coreCollider = core.GetComponent<Collider>();
        health = GetComponent<EnemyHealth>();
        parentCollider = parent.GetComponent<Collider>();

        coreRigid.isKinematic = true;
        corePickUp.enabled = false;
        coreCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Molotov"))
        {
            if (health.IsDead() == true)
            {
                isBurning = true;
                Debug.Log("Burning");
                InitializeBurn();
            }
        }
    }

    public void InitializeBurn()
    {
        parentCollider.enabled = false;
        core.transform.parent = null;
        fire.SetActive(true);
        coreCollider.enabled = true;
        coreRigid.isKinematic = false;
        corePickUp.enabled = true;
        isBurned = true;
        StartCoroutine(DisableAfterBurn(burnParticles.main.duration));

    }

    private IEnumerator DisableAfterBurn(float delay)
    {
        yield return new WaitForSeconds(delay);

        parent.SetActive(false);
    }

    public bool IsBurned()
    {
        return isBurned;
    }
}
