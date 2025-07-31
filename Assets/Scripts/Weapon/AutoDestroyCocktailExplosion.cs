using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyCocktailExplosion : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
    }
}
