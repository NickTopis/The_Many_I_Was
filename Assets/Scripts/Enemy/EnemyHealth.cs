using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float hitPoints = 100f;
    [SerializeField] bool testMode = false;
    [SerializeField] EnemyBurn enemyBurn;
    public bool isDead = false;
    private void Start()
    {
        enemyBurn.enabled = false;
        if (testMode) { hitPoints = 1f; }
    }
    public bool IsDead() { return isDead; }
    public void TakeDamage(int damage)
    {
        BroadcastMessage("OnDamageTaken");
        hitPoints -= damage;
        if(hitPoints <= 0 )
        {
            Die();
        }
    }
    private void Update()
    {
        if (isDead)
        {
            enemyBurn.enabled = true;
        }
    }
    private void Die()
    {
        if (isDead) { return; }
        enemyBurn.enabled =true;
        isDead = true;
        GetComponent<Animator>().SetTrigger("die");
    }

    public void ForceDeadState()
    {
        isDead = true;
        enemyBurn.enabled = true;
        GetComponent<Animator>().SetTrigger("die");
    }

}
