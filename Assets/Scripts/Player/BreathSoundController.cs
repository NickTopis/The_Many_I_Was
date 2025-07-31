using UnityEngine;
using System.Collections;

public class BreathSoundController : MonoBehaviour
{
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] AudioSource walkingBreath;
    [SerializeField] AudioSource runningBreath;
    [SerializeField] float fadeDuration = 0.5f;

    private AudioSource currentBreath;
    private Coroutine fadeCoroutine;

    void Start()
    {
        walkingBreath.volume = 0f;
        runningBreath.volume = 0f;
    }

    void Update()
    {
        bool isWalking = firstPersonController.isWalking;
        bool isRunning = firstPersonController.isSprinting;

        if (isRunning)
        {
            SwitchToBreath(runningBreath);
        }
        else if (isWalking)
        {
            SwitchToBreath(walkingBreath);
        }
        else
        {
            SwitchToBreath(walkingBreath);
        }
    }

    void SwitchToBreath(AudioSource newBreath)
    {
        if (newBreath == currentBreath) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeBreath(currentBreath, newBreath));
        currentBreath = newBreath;
    }

    IEnumerator FadeBreath(AudioSource from, AudioSource to)
    {
        float time = 0f;
        float fromStartVol = from != null ? from.volume : 0f;

        if (to != null && !to.isPlaying) to.Play();

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            if (from != null) from.volume = Mathf.Lerp(fromStartVol, 0f, t);
            if (to != null) to.volume = Mathf.Lerp(0f, 1f, t);

            time += Time.deltaTime;
            yield return null;
        }

        if (from != null)
        {
            from.volume = 0f;
            from.Stop();
        }

        if (to != null)
        {
            to.volume = 1f;
        }
    }
}
