using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

[ExecuteAlways]
public class EnemyID : MonoBehaviour
{
    [SerializeField] public string uniqueID;

    public string ID => uniqueID;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (string.IsNullOrEmpty(uniqueID) || !IsUnique(uniqueID))
            {
                uniqueID = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(this);
            }
        }
    }

    private bool IsUnique(string id)
    {
        EnemyID[] all = FindObjectsOfType<EnemyID>(true);
        foreach (var other in all)
        {
            if (other != this && other.uniqueID == id)
                return false;
        }
        return true;
    }
#endif
}
