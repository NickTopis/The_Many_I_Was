using UnityEngine;

public class FlashlightStateManager : MonoBehaviour
{
    [SerializeField] GameObject flashlightPickupInScene;
    [SerializeField] FlashLightController flashlightController;
    [SerializeField] GameObject objectOnPlayer;
    [SerializeField] GameObject textObj;
    [SerializeField] Light myLight;
    [SerializeField] Material volumeMaterial;
    [SerializeField] GameObject flashlight;

    private float volumeOpacity = 0.5f;
    private bool pickedUp = false;
    private bool flashlightIsOn =true;

    public void SetPickedUp(bool state)
    {
        pickedUp = state;
    }

    public bool IsPickedUp()
    {
        return pickedUp;
    }

    public void ApplyFlashlightState()
    {
        if (pickedUp)
        {
            if (flashlightPickupInScene != null)
                Destroy(flashlightPickupInScene);

            if (objectOnPlayer != null)
            {
                objectOnPlayer.SetActive(true);
                flashlightController.enabled = true;
            }

            if (textObj != null)
                textObj.SetActive(true);
        }
        else
        {
            if (objectOnPlayer != null)
                objectOnPlayer.SetActive(false);
            if (textObj != null)
                textObj.SetActive(false);
            if (flashlightPickupInScene != null)
                flashlightPickupInScene.SetActive(true);
        }

        flashlight.SetActive(flashlightIsOn);
    }

    public void SetLightState(float intensity, float angle, float volume)
    {
        myLight.intensity = intensity;
        myLight.spotAngle = angle;
        volumeOpacity = volume;
        volumeMaterial.SetFloat("_Opacity", volumeOpacity);
    }

    public float GetLightIntensity() => myLight.intensity;
    public float GetLightAngle() => myLight.spotAngle;
    public float GetVolumeOpacity() => volumeOpacity;
    public bool IsLightOn() => flashlightIsOn;
    public void SetLightOn(bool on)
    {
        flashlightIsOn = on;
    }
    public void SetVolumeOpacity(float value)
    {
        volumeOpacity = value;
        volumeMaterial.SetFloat("_Opacity", volumeOpacity);
    }
}
