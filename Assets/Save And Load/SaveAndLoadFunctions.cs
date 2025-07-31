using System.Collections;
using UnityEngine;

public class SaveAndLoadFunctions : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Ammo ammo;
    [SerializeField] private FlashlightStateManager flashlightStateManager;
    [SerializeField] private GunPickUpStateManager gunStateManager;
    [SerializeField] private float saveTime = 60f;
    [SerializeField] private BookPickUpManager bookPickUpManager;
    [SerializeField] private StartingSequence startingSequence;

    private bool isSaving = false;

    private void Awake()
    {
        if (player == null || playerHealth == null || ammo == null || flashlightStateManager == null || gunStateManager == null || bookPickUpManager == null)
        {
            Debug.LogWarning("Missing references in SaveAndLoadFunctions. Please assign all fields in the inspector.");
        }
    }

    private IEnumerator Start()
    {
        while (PickUpManager.Instance == null)
        {
            Debug.Log("Waiting for PickUpManager...");
            yield return null;
        }

        LoadGame();
        // Uncomment if you want autosave
        // StartCoroutine(AutoSaveRoutine(saveTime));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            DeleteSaveData();
        }
    }

    public void SaveGame()
    {
        if (isSaving) return; // prevent overlapping saves
        StartCoroutine(SaveRoutine());
    }

    private IEnumerator SaveRoutine()
    {
        isSaving = true;
        SaveSystem.SavePlayer(playerHealth, ammo, gunStateManager, flashlightStateManager, startingSequence, bookPickUpManager);
        FindObjectOfType<EnemySaveManager>().SaveToFile();
        Debug.Log("Game Saved");
        yield return null;
        isSaving = false;
    }

    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null) return;

        startingSequence.startingSequence = data.startingSequenceState;

        if (data.position.Length == 3)
        {
            player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        }

        if (data.rotation.Length == 3)
        {
            player.transform.eulerAngles = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);
        }

        playerHealth.hitPoints = data.health;

        foreach (var entry in data.ammoData)
        {
            if (System.Enum.TryParse(entry.ammoType, out AmmoType ammoType))
            {
                ammo.SetAmmoSlot(ammoType, entry.ammoAmount);
            }
        }

        gunStateManager.SetGunPickedUp(data.gunPickedUp);
        gunStateManager.ApplyGunState();

        flashlightStateManager.SetPickedUp(data.flashlightPickedUp);
        flashlightStateManager.SetLightState(data.flashlightIntensity, data.flashlightAngle, data.flashlightVolume);
        flashlightStateManager.SetVolumeOpacity(data.flashlightVolume);
        flashlightStateManager.ApplyFlashlightState();

        bookPickUpManager.SetBookPickedUp(data.gotBook);
        bookPickUpManager.SetPieces(data.piecesShown);
        bookPickUpManager.ApplyBookState();

        PickUpManager.Instance.LoadPickedUpIDs(data.pickedUpIDs);
        FindObjectOfType<EnemySaveManager>().LoadFromFile();
        Debug.Log("Game Loaded");
    }

    public void DeleteSaveData()
    {
        SaveSystem.DeletePlayerData();
        FindObjectOfType<EnemySaveManager>().DeleteSaveFile();
        Debug.Log("Save data deleted.");
    }

    private IEnumerator AutoSaveRoutine(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            SaveGame();
        }
    }
}
