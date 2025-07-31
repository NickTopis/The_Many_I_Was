using UnityEngine;
using System.Collections.Generic;

public class WatcherSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera playerCamera;
    public GameObject watcherPrefab;

    [Header("Spawn Points")]
    public List<Transform> spawnPoints;

    [Header("Settings")]
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 50f;
    public float spawnCooldown = 30f;
    public bool requireNotVisibleByRay = true;

    [Header("Debug")]
    public bool logSpawns = true;

    private GameObject currentWatcher;
    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time >= nextSpawnTime && currentWatcher == null)
        {
            if (TrySpawnClosestPoint())
            {
                nextSpawnTime = Time.time + spawnCooldown;
                if (logSpawns) Debug.Log("Watcher spawned at closest valid spawn point.");
            }
        }
    }

    bool TrySpawnClosestPoint()
    {
        if (spawnPoints == null || spawnPoints.Count == 0) return false;

        Transform closestPoint = null;
        float closestDist = float.MaxValue;

        foreach (var t in spawnPoints)
        {
            if (!t) continue;

            float dist = Vector3.Distance(player.position, t.position);
            if (dist < minSpawnDistance) continue;
            if (maxSpawnDistance > 0f && dist > maxSpawnDistance) continue;

            if (IsPointVisibleToCamera(t.position)) continue;

            if (requireNotVisibleByRay && HasLineOfSight(playerCamera.transform.position, t.position))
                continue;

            if (dist < closestDist)
            {
                closestDist = dist;
                closestPoint = t;
            }
        }

        if (closestPoint == null) return false;

        SpawnWatcherAt(closestPoint);
        return true;
    }

    void SpawnWatcherAt(Transform spawnPoint)
    {
        currentWatcher = Instantiate(watcherPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
    }

    bool IsPointVisibleToCamera(Vector3 point)
    {
        Vector3 viewportPos = playerCamera.WorldToViewportPoint(point);
        return (viewportPos.x >= 0 && viewportPos.x <= 1 &&
                viewportPos.y >= 0 && viewportPos.y <= 1 &&
                viewportPos.z > 0);
    }

    bool HasLineOfSight(Vector3 from, Vector3 to)
    {
        Vector3 dir = to - from;
        float dist = dir.magnitude;
        dir.Normalize();

        if (Physics.Raycast(from, dir, out RaycastHit hit, dist))
        {
            // If ray hits anything before the target point, no line of sight
            // Assuming watcher and spawn points don't have colliders that block
            // Or add layer mask if needed to exclude some layers
            return false;
        }
        return true;
    }

    void OnDrawGizmosSelected()
    {
        if (spawnPoints == null) return;

        Gizmos.color = Color.cyan;
        foreach (var t in spawnPoints)
        {
            if (t == null) continue;
            Gizmos.DrawWireSphere(t.position, 0.5f);
        }

        if (player != null)
        {
            Gizmos.color = new Color(1, 0, 0, 0.2f);
            Gizmos.DrawWireSphere(player.position, minSpawnDistance);
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            if (maxSpawnDistance > 0f)
                Gizmos.DrawWireSphere(player.position, maxSpawnDistance);
        }
    }
}
