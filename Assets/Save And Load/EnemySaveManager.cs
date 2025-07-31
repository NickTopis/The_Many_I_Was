using System.Collections.Generic;
using UnityEngine;

public class EnemySaveManager : MonoBehaviour
{
    [SerializeField] private EnemyInstances enemyInstances;

    public void SaveToFile()
    {
        List<EnemySaveData> data = CollectEnemyData();
        string json = JsonUtility.ToJson(new EnemyDataWrapper { enemies = data }, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/enemy_save.json", json);
    }

    public void LoadFromFile()
    {
        string path = Application.persistentDataPath + "/enemy_save.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            EnemyDataWrapper wrapper = JsonUtility.FromJson<EnemyDataWrapper>(json);
            LoadEnemyData(wrapper.enemies);
        }
    }

    [System.Serializable]
    public class EnemyDataWrapper
    {
        public List<EnemySaveData> enemies;
    }


    public List<EnemySaveData> CollectEnemyData()
    {
        List<EnemySaveData> enemyDataList = new List<EnemySaveData>();

        foreach (GameObject enemy in enemyInstances.GetEnemies())
        {
            EnemyID id = enemy.GetComponent<EnemyID>();
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            EnemyBurn burn = enemy.GetComponent<EnemyBurn>();

            if (id != null && health != null && burn != null)
            {
                enemyDataList.Add(new EnemySaveData
                {
                    uniqueID = id.uniqueID,
                    hitPoints = health.hitPoints,
                    isDead = health.isDead,
                    isBurning = burn.isBurning,
                    isBurned = burn.isBurned
                });
            }
        }

        return enemyDataList;
    }

    public void LoadEnemyData(List<EnemySaveData> savedData)
    {
        foreach (GameObject enemy in enemyInstances.GetEnemies())
        {
            EnemyID id = enemy.GetComponent<EnemyID>();
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            EnemyBurn burn = enemy.GetComponent<EnemyBurn>();

            if (id == null || health == null || burn == null) continue;

            EnemySaveData data = savedData.Find(e => e.uniqueID == id.uniqueID);
            if (data != null)
            {
                health.hitPoints = data.hitPoints;
                health.isDead = data.isDead;
                burn.isBurning = data.isBurning;
                burn.isBurned = data.isBurned;

                // Apply visual effects if dead
                if (data.isDead)
                {
                    health.ForceDeadState();
                }
            }
        }
    }

    public void DeleteSaveFile()
    {
        string path = Application.persistentDataPath + "/enemy_save.json";
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
            Debug.Log("Enemy save file deleted.");
        }
        else
        {
            Debug.LogWarning("No enemy save file found to delete.");
        }
    }

}
