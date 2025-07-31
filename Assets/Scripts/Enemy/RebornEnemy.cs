using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RebornEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    public void InitializeReborn()
    {
        Instantiate(enemyPrefab,transform.position, Quaternion.identity);
        Debug.Log("Enemy Reborn");
    }
}
