#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeightedRandomSpawner))]
public class WeightedRandomSpawnerEditor : Editor
{
    SerializedProperty spawnPointsProp;
    SerializedProperty prefabOptionsProp;
    SerializedProperty emptyChanceProp;
    SerializedProperty useFixedSeedProp;
    SerializedProperty seedProp;
    SerializedProperty parentToSpawnPointProp;
    SerializedProperty clearBeforeSpawnProp;
    SerializedProperty logDebugProp;

    void OnEnable()
    {
        spawnPointsProp = serializedObject.FindProperty("spawnPoints");
        prefabOptionsProp = serializedObject.FindProperty("prefabOptions");
        emptyChanceProp = serializedObject.FindProperty("emptyChance");
        useFixedSeedProp = serializedObject.FindProperty("useFixedSeed");
        seedProp = serializedObject.FindProperty("seed");
        parentToSpawnPointProp = serializedObject.FindProperty("parentToSpawnPoint");
        clearBeforeSpawnProp = serializedObject.FindProperty("clearBeforeSpawn");
        logDebugProp = serializedObject.FindProperty("logDebug");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(spawnPointsProp, true);
        EditorGUILayout.PropertyField(prefabOptionsProp, true);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(emptyChanceProp);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(useFixedSeedProp);
        if (useFixedSeedProp.boolValue)
        {
            EditorGUILayout.PropertyField(seedProp);
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(parentToSpawnPointProp);
        EditorGUILayout.PropertyField(clearBeforeSpawnProp);
        EditorGUILayout.PropertyField(logDebugProp);

        EditorGUILayout.Space();
        DrawWeightSummary();

        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn Using Current Seed"))
        {
            foreach (var t in targets)
            {
                var spawner = (WeightedRandomSpawner)t;
                spawner.SpawnAll(false);
            }
        }
        if (GUILayout.Button("Reseed & Spawn"))
        {
            foreach (var t in targets)
            {
                var spawner = (WeightedRandomSpawner)t;
                spawner.SpawnAll(true); // reseed
            }
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Clear Spawned"))
        {
            foreach (var t in targets)
            {
                var spawner = (WeightedRandomSpawner)t;
                spawner.ClearSpawned();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DrawWeightSummary()
    {
        var spawner = (WeightedRandomSpawner)target;
        if (spawner.prefabOptions == null || spawner.prefabOptions.Count == 0) return;

        float total = 0f;
        foreach (var opt in spawner.prefabOptions)
            if (opt != null && opt.prefab != null && opt.weight > 0f)
                total += opt.weight;

        if (total <= 0f)
        {
            EditorGUILayout.HelpBox("All weights are 0. Nothing will spawn (except empty rolls).", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Weighted Probabilities:");
        foreach (var opt in spawner.prefabOptions)
        {
            if (opt == null || opt.prefab == null) continue;
            float pct = (opt.weight <= 0) ? 0f : (opt.weight / total) * 100f;
            EditorGUILayout.LabelField($"  • {opt.prefab.name}: {pct:F1}%");
        }
    }
}
#endif