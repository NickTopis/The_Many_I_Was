using UnityEngine;

public class WhatcherEnemy : MonoBehaviour
{
    private GameObject playerObj;
    public Transform player;

    [Header("Fade Settings")]
    public float fadeStartDistance = 40f;   // Distance where opacity starts fading
    public float fadeEndDistance = 5f;      // Distance where opacity is 0
    public float fadeSmooth = 5f;           // Smooth transition speed
    public bool destroyOnFadeOut = true;

    private float currentOpacity = 1f;
    private MaterialPropertyBlock mpb;
    private Renderer[] renderers;

    void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.transform;
        renderers = GetComponentsInChildren<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (player == null) return;

        RotateTowardsPlayer();
        UpdateOpacity();
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Only rotate on Y axis
        if (direction != Vector3.zero)
            transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 5f);
    }

    private void UpdateOpacity()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        // Correct fade direction: 1 when far, 0 when close
        float targetOpacity = Mathf.InverseLerp(fadeEndDistance, fadeStartDistance, dist);

        currentOpacity = Mathf.Lerp(currentOpacity, targetOpacity, Time.deltaTime * fadeSmooth);

        foreach (Renderer r in renderers)
        {
            r.GetPropertyBlock(mpb);
            mpb.SetFloat("_Opacity", currentOpacity);
            r.SetPropertyBlock(mpb);
        }

        if (destroyOnFadeOut && currentOpacity <= 0.01f && dist <= fadeEndDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw fading start range (green)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fadeStartDistance);

        // Draw destruction range (red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fadeEndDistance);
    }
}
