using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class WhisperingSoundController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] whisperClips;
    [SerializeField] float minDelay = 45f;
    [SerializeField] float maxDelay = 120f;

    [Header("Mixer Groups")]
    [SerializeField] AudioMixerGroup normalGroup;
    [SerializeField] AudioMixerGroup reverbGroup;

    void Start()
    {
        StartCoroutine(PlayWhispersRandomly());
    }

    IEnumerator PlayWhispersRandomly()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSecondsRealtime(delay);

            if (whisperClips.Length == 0 || audioSource == null)
                continue;

            if (audioSource.isPlaying)
                yield return new WaitWhile(() => audioSource.isPlaying);

            AudioClip originalClip = whisperClips[Random.Range(0, whisperClips.Length)];
            int mode = Random.Range(0, 3); // 0 = normal, 1 = reverb, 2 = reverse

            AudioClip clipToPlay = originalClip;

            switch (mode)
            {
                case 0: // Normal
                    audioSource.outputAudioMixerGroup = normalGroup;
                    break;

                case 1: // Reverb
                    audioSource.outputAudioMixerGroup = reverbGroup;
                    break;

                case 2: // Reverse
                    clipToPlay = ReverseAudioClip(originalClip);
                    audioSource.outputAudioMixerGroup = normalGroup;
                    break;
            }

            audioSource.clip = clipToPlay;
            audioSource.pitch = 1f;
            audioSource.panStereo = Random.Range(-0.4f, 0.4f);
            audioSource.Play();

            yield return new WaitWhile(() => audioSource.isPlaying);
        }
    }

    private AudioClip ReverseAudioClip(AudioClip clip)
    {
        float[] data = new float[clip.samples * clip.channels];
        clip.GetData(data, 0);

        System.Array.Reverse(data); // clean and fast

        AudioClip reversed = AudioClip.Create(clip.name + "_reversed", clip.samples, clip.channels, clip.frequency, false);
        reversed.SetData(data, 0);
        return reversed;
    }
}
