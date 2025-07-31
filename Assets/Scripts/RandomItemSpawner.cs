using UnityEngine;

[System.Serializable]
public class SpawnablePrefab
{
    public GameObject prefab;
    [Range(0f, 1f)]
    public float spawnChance = 1f; // Probability (0 to 1)
}

public class RandomItemSpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Prefabs With Chance")]
    [SerializeField] private SpawnablePrefab[] prefabOptions;

    [Header("Empty Chance")]
    [Range(0f, 1f)]
    [SerializeField] private float emptyChance = 0.2f;

    [SerializeField] private bool spawnOnStart = true;

    void Start()
    {
        if (spawnOnStart)
            SpawnAll();
    }

    [ContextMenu("Spawn All")]
    public void SpawnAll()
    {
        foreach (Transform point in spawnPoints)
        {
            if (Random.value < emptyChance)
            {
                continue; // leave empty
            }

            GameObject prefab = GetRandomPrefab();
            if (prefab != null)
            {
                Instantiate(prefab, point.position, point.rotation);
            }
        }
    }

    private GameObject GetRandomPrefab()
    {
        float totalWeight = 0f;
        foreach (var p in prefabOptions)
        {
            totalWeight += p.spawnChance;
        }

        float randomValue = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var p in prefabOptions)
        {
            cumulative += p.spawnChance;
            if (randomValue <= cumulative)
            {
                return p.prefab;
            }
        }
        return null; // fallback
    }
}
