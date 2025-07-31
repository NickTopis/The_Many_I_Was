using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroChangeScene : MonoBehaviour
{
    public float videoDuration = 2f; 

    private VideoPlayer videoPlayer;

    void Start()
    {
        StartCoroutine(PlayVideoAndLoadScene());
    }

    private IEnumerator PlayVideoAndLoadScene()
    {

        yield return new WaitForSeconds(videoDuration + 1);

        SceneManager.LoadScene(1);
    }
}
