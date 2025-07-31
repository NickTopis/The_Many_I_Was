using UnityEngine;

public class FootStepControl : MonoBehaviour
{
    [SerializeField] private AudioSource audioPlayer;

    [SerializeField] public AudioClip walkClip;
    [SerializeField] public AudioClip runClip;

    [SerializeField] private float walkSpeedThreshold = 0.1f;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    void Awake()
    {
        if (audioPlayer == null)
            audioPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;

        if (isMoving)
        {
            AudioClip targetClip = Input.GetKey(runKey) ? runClip : walkClip;

            if (audioPlayer.clip != targetClip)
                audioPlayer.clip = targetClip;

            if (!audioPlayer.isPlaying)
                audioPlayer.Play();
        }
        else
        {
            if (audioPlayer.isPlaying)
                audioPlayer.Stop();
        }
    }
}
