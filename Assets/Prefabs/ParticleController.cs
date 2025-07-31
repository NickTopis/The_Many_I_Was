using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 20f;
    public float maxRateOverTime = 20f;
    public float smoothSpeed = 5f; // How fast the value transitions

    private ParticleSystem ps;
    private ParticleSystem.EmissionModule emission;
    private float currentRate = 0f;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        emission = ps.emission;
    }

    void Update()
    {
        if (!player) return;

        // Calculate target rate based on distance
        float dist = Vector3.Distance(player.position, transform.position);
        float normalized = Mathf.Clamp01(1 - (dist / maxDistance)); // 1 close, 0 far
        float targetRate = maxRateOverTime * normalized;

        // Smoothly move currentRate toward targetRate
        currentRate = Mathf.Lerp(currentRate, targetRate, Time.deltaTime * smoothSpeed);
        emission.rateOverTime = currentRate;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!player) return;

        // Draw a sphere around the player to visualize maxDistance
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, maxDistance);

        // Optional: Draw a line to the particle system
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(player.position, transform.position);
    }
#endif
}
