using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class RenderFeaturesONOff : MonoBehaviour
{
    [Header("Video Settings")]
    public float introVideoDuration = 2f;

    [Header("Renderer Feature Settings")]
    public UniversalRendererData rendererData; // Assign your Forward Renderer here
    public string featureName = "FullScreenPassRendererFeature";

    private ScriptableRendererFeature targetFeature;

    void Start()
    {
        Cursor.visible = false;

        // Find the renderer feature by name
        foreach (var feature in rendererData.rendererFeatures)
        {
            if (feature != null && feature.name == featureName)
            {
                targetFeature = feature;
                break;
            }
        }

        if (targetFeature != null)
        {
            // Disable the feature at the start
            targetFeature.SetActive(false);
        }
        else
        {
            Debug.LogError("Renderer Feature not found! Check the feature name.");
        }

        // Start the video and scene change sequence
        StartCoroutine(PlayIntroAndLoadScene());
    }

    private IEnumerator PlayIntroAndLoadScene()
    {
        // Wait for the video to finish
        yield return new WaitForSeconds(introVideoDuration + 1);

        // Enable the renderer feature after the video ends
        if (targetFeature != null)
        {
            targetFeature.SetActive(true);
        }

        // Load the next scene
        SceneManager.LoadScene(1);
    }
}
