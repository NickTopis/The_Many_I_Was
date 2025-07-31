using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveHandler : MonoBehaviour
{
    [SerializeField] float dissolveRate = 0.0125f;
    [SerializeField] float refreshRate = 0.025f;

    [SerializeField] SkinnedMeshRenderer skinnedMesh;
        
    private Material[] materials;
    public Material material;
    [SerializeField] EnemyHealth health;

    bool executed = false;
    void Start()
    {
        if(skinnedMesh != null)
        {
            materials = skinnedMesh.materials;
        }

        health = FindObjectOfType<EnemyHealth>();
    }

    void Update()
    {
        if (health.IsDead() && !executed)
        {
            ExecuteDissolve();
            executed = true;
       }
    }

    public void ExecuteDissolve()
    {
        StartCoroutine(DissolveCo());
    }

    IEnumerator DissolveCo()
    {
        if(materials.Length > 0)
        {
            
            float counter = 0;
            while (materials[0].GetFloat("_DissolveAmount") < 1)
            {
                
                counter += dissolveRate;
                for (int i = 0; i < materials.Length; i++)
                {
                    material.SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
