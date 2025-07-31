using UnityEngine;
using System.Collections;

public class FaultyBulbFlicker : MonoBehaviour
{
    [SerializeField] private Light targetLight;
    [SerializeField] private AudioSource hum;
    [SerializeField] private AudioSource flicker;

    [Header("Base")]
    public float baseIntensity = 1f;
    public bool useLightInitialIntensity = true;

    [Header("Burst Timing (seconds)")]
    public Vector2 waitBetweenBursts = new Vector2(1.5f, 5f);   // Idle time before next flicker burst
    public Vector2 burstDuration = new Vector2(0.2f, 1.2f);     // How long a burst lasts

    [Header("During Burst")]
    [Range(0f, 60f)] public float flickersPerSecond = 20f;       // How many intensity hits per second in burst
    public Vector2 flickerRange = new Vector2(0.1f, 1.4f);       // Multiplier of base

    [Header("Blackout")]
    [Range(0f, 1f)] public float blackoutChancePerFlicker = 0.05f;
    public float blackoutMin = 0.05f;
    public float blackoutMax = 0.25f;

    [Header("Return")]
    public float postBurstReturnSpeed = 10f;                     // Lerp back after burst

    private bool _isBursting;
    private bool _isBlackout;
    private float _currentIntensity;
    private Coroutine _routine;

    void Start()
    {
        if (targetLight == null)
        {
            Debug.LogError("FaultyBulbFlicker: No Light assigned.");
            enabled = false;
            return;
        }

        if (useLightInitialIntensity)
            baseIntensity = targetLight.intensity;

        _currentIntensity = baseIntensity;
        targetLight.intensity = baseIntensity;

        if (hum != null)
        {
            hum.Play();
        }

        _routine = StartCoroutine(FlickerController());
    }

    IEnumerator FlickerController()
    {
        while (true)
        {
            // --- Idle phase ---
            float wait = Random.Range(waitBetweenBursts.x, waitBetweenBursts.y);
            yield return new WaitForSeconds(wait);

            // --- Burst phase ---
            float dur = Random.Range(burstDuration.x, burstDuration.y);
            yield return StartCoroutine(DoBurst(dur));

            // --- Smooth settle back to base ---
            yield return StartCoroutine(ReturnToBase());
        }
    }

    IEnumerator DoBurst(float duration)
    {
        _isBursting = true;
        if (flicker != null)
        {
            FlickerSound(true);
        }
        float endTime = Time.time + duration;
        float flickerInterval = (flickersPerSecond <= 0f) ? duration : 1f / flickersPerSecond;

        while (Time.time < endTime)
        {
            if (Random.value < blackoutChancePerFlicker)
            {
                yield return StartCoroutine(DoBlackout());
            }
            else
            {
                float mult = Random.Range(flickerRange.x, flickerRange.y);
                _currentIntensity = Mathf.Max(0f, baseIntensity * mult);
                targetLight.intensity = _currentIntensity;
                yield return new WaitForSeconds(flickerInterval * Random.Range(0.5f, 1.5f)); // jitter
            }
        }

        _isBursting = false;
    }

    IEnumerator DoBlackout()
    {
        _isBlackout = true;
        float dur = Random.Range(blackoutMin, blackoutMax);
        targetLight.intensity = 0f;
        yield return new WaitForSeconds(dur);
        _isBlackout = false;
    }

    IEnumerator ReturnToBase()
    {
        if (flicker != null)
        {
            FlickerSound(false);
        }
        float t = 0f;
        float startIntensity = targetLight.intensity;
        while (t < 1f)
        {
            t += Time.deltaTime * postBurstReturnSpeed;
            targetLight.intensity = Mathf.Lerp(startIntensity, baseIntensity, t);
            yield return null;
        }
        _currentIntensity = baseIntensity;
    }

    // --- Optional public controls ---
    public void ForceBurst(float durationOverride = -1f)
    {
        if (!gameObject.activeInHierarchy || targetLight == null) return;
        if (_routine != null) StopCoroutine(_routine);
        float dur = (durationOverride > 0f) ? durationOverride : Random.Range(burstDuration.x, burstDuration.y);
        _routine = StartCoroutine(ForceBurstRoutine(dur));
    }

    IEnumerator ForceBurstRoutine(float dur)
    {
        yield return StartCoroutine(DoBurst(dur));
        yield return StartCoroutine(ReturnToBase());
        _routine = StartCoroutine(FlickerController());
    }

    void FlickerSound(bool flickering)
    {
        if (flickering == true)
        {
            flicker.Play();
            Debug.Log("Flickering");
        }
        if(flickering == false)
        {
            flicker.Stop();
            Debug.Log("Not Flickering");
        }
    }
}
