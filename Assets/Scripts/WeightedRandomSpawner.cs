using Palmmedia.ReportGenerator.Core;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class WeightedPrefab
{
    public GameObject prefab;
    [Min(0f)] public float weight = 1f; // relative probability; 0 = never
}

/// <summary>
/// Editor-friendly weighted spawner that can spawn into the scene *outside of Play Mode*.
/// Use the custom inspector buttons to Spawn, Reseed, and Clear.
/// </summary>
[ExecuteAlways]
public class WeightedRandomSpawner : MonoBehaviour
{
    [Header("Spawn Points (any size)")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Prefabs With Weights")]
    public List<WeightedPrefab> prefabOptions = new List<WeightedPrefab>();

    [Header("Empty Chance (0-1; per spawn point)")]
    [Range(0f, 1f)] public float emptyChance = 0.2f;

    [Header("Seeding")]
    public bool useFixedSeed = true;
    public int seed = 12345;

    [Header("Behavior")]
    public bool parentToSpawnPoint = true;
    public bool clearBeforeSpawn = true;
    public bool logDebug = false;

    // Track spawned so we can clear cleanly (serialized so it survives domain reload in editor)
    [HideInInspector][SerializeField] private List<GameObject> spawnedInstances = new List<GameObject>();

    // --- PUBLIC API ---
    public void SpawnAll(bool reseed = false)
    {
        if (reseed)
        {
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
        DoSpawn();
    }

    public void ClearSpawned()
    {
        InternalClearSpawned();
    }

    // --- CORE ---
    private void DoSpawn()
    {
        if (!Validate()) return;

        if (useFixedSeed)
            UnityEngine.Random.InitState(seed);

        if (clearBeforeSpawn)
            InternalClearSpawned();

        // precompute total weight
        float totalWeight = 0f;
        foreach (var opt in prefabOptions)
            if (opt != null && opt.prefab != null && opt.weight > 0f)
                totalWeight += opt.weight;

        spawnedInstances ??= new List<GameObject>();

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Transform p = spawnPoints[i];
            if (!p) continue;

            // Empty roll
            if (UnityEngine.Random.value < emptyChance)
            {
                if (logDebug) Debug.Log($"[WeightedRandomSpawner] EMPTY at {i}:{p.name}", this);
                continue;
            }

            GameObject picked = PickWeighted(totalWeight);
            if (picked == null) continue;

            Transform parent = parentToSpawnPoint ? p : null;
            GameObject inst = SafeInstantiate(picked, p.position, p.rotation, parent);
            spawnedInstances.Add(inst);
            if (logDebug) Debug.Log($"[WeightedRandomSpawner] Spawned {picked.name} at {p.name}", inst);
        }
    }

    private GameObject PickWeighted(float totalWeight)
    {
        if (totalWeight <= 0f) return null;
        float r = UnityEngine.Random.value * totalWeight;
        float cumulative = 0f;
        for (int i = 0; i < prefabOptions.Count; i++)
        {
            var opt = prefabOptions[i];
            if (opt == null || opt.prefab == null || opt.weight <= 0f) continue;
            cumulative += opt.weight;
            if (r <= cumulative)
                return opt.prefab;
        }
        return null;
    }

    private void InternalClearSpawned()
    {
        // Destroy using Undo in editor; Destroy in play mode.
        if (spawnedInstances != null)
        {
            for (int i = spawnedInstances.Count - 1; i >= 0; i--)
            {
                var go = spawnedInstances[i];
                if (go == null) { spawnedInstances.RemoveAt(i); continue; }
                SafeDestroy(go);
                spawnedInstances.RemoveAt(i);
            }
        }

        // If parented to spawn points, also remove any remaining children that match spawned prefabs (optional).
        if (parentToSpawnPoint)
        {
            foreach (Transform p in spawnPoints)
            {
                if (!p) continue;
                for (int i = p.childCount - 1; i >= 0; i--)
                {
                    var child = p.GetChild(i).gameObject;
                    SafeDestroy(child);
                }
            }
        }
    }

    private bool Validate()
    {
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("[WeightedRandomSpawner] No spawn points assigned.", this);
            return false;
        }
        if (prefabOptions == null || prefabOptions.Count == 0)
        {
            Debug.LogError("[WeightedRandomSpawner] No prefab options assigned.", this);
            return false;
        }
        return true;
    }

    // --- SAFE CREATE/DESTROY HELPERS ---
    private GameObject SafeInstantiate(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            GameObject inst = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent ?? this.transform);
            inst.transform.SetPositionAndRotation(pos, rot);
            UnityEditor.Undo.RegisterCreatedObjectUndo(inst, "Spawn Prefab");
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
            return inst;
        }
#endif
        return Instantiate(prefab, pos, rot, parent);
    }

    private void SafeDestroy(GameObject go)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.Undo.DestroyObjectImmediate(go);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
            return;
        }
#endif
        Destroy(go);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Ensure no negative weights
        if (prefabOptions != null)
        {
            foreach (var opt in prefabOptions)
                if (opt != null && opt.weight < 0f) opt.weight = 0f;
        }
    }
#endif

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (spawnPoints == null) return;
        Gizmos.color = Color.yellow;
        foreach (var t in spawnPoints)
        {
            if (!t) continue;
            Gizmos.DrawWireSphere(t.position, 0.25f);
        }
    }
#endif
}

