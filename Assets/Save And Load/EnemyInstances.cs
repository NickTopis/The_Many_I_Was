using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstances : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    void Start()
    {
        foreach (GameObject enemy in enemies)
        {
            EnemyID enemyID = enemy.GetComponent<EnemyID>();
            if(enemyID != null)
            {
                Debug.Log(enemy.name + " ID is: " + enemyID.uniqueID);
            }

            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                Debug.Log(enemy.name + " HP: " + health.hitPoints);
                Debug.Log(enemy.name + " is dead: " + health.isDead);
            }

            EnemyBurn burn = enemy.GetComponent<EnemyBurn>();
            if (burn != null)
            {
                Debug.Log(enemy.name + " is burning: " + burn.isBurning);
                Debug.Log(enemy.name + " burned: " + burn.isBurned);
            }


        }
    }

    public GameObject[] GetEnemies()
    {
        return enemies;
    }

}
