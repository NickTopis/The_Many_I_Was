using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float[] position;
    public float[] rotation;
    public int level;
    public float experience;
    public string playerName;
    public bool gunPickedUp;

    public List<AmmoEntry> ammoData;

    public bool flashlightPickedUp;
    public float flashlightIntensity;
    public float flashlightAngle;
    public float flashlightVolume;
    public bool flashlightIsOn;
    public List<string> pickedUpIDs;
    public bool startingSequenceState;
    public bool gotBook;
    public int piecesShown;

    [System.Serializable]
    public class AmmoEntry
    {
        public string ammoType;
        public int ammoAmount;
    }

    public PlayerData(PlayerHealth player, Ammo ammo, GunPickUpStateManager gunState, FlashlightStateManager flashlightState, StartingSequence startingSequence, BookPickUpManager bookPickUpManager)
    {
        health = player.hitPoints;

        Vector3 pos = player.transform.position;
        position = new float[3] { pos.x, pos.y, pos.z };

        Vector3 rot = player.transform.eulerAngles;
        rotation = new float[3] { rot.x, rot.y, rot.z };

        gunPickedUp = gunState.IsGunPickedUp();

        ammoData = new List<AmmoEntry>();
        foreach (var slot in ammo.GetAllAmmoSlots())
        {
            ammoData.Add(new AmmoEntry
            {
                ammoType = slot.ammoType.ToString(),
                ammoAmount = slot.ammoAmount
            });
        }

        flashlightPickedUp = flashlightState.IsPickedUp();
        flashlightIntensity = flashlightState.GetLightIntensity();
        flashlightAngle = flashlightState.GetLightAngle();
        flashlightVolume = flashlightState.GetVolumeOpacity();
        flashlightIsOn = flashlightState.IsLightOn();
        pickedUpIDs = PickUpManager.Instance.GetPickedUpIDs();
        startingSequenceState = startingSequence.startingSequence;
        gotBook = bookPickUpManager.IsBookPickedUp();
        piecesShown = bookPickUpManager.ShownPieces();
        
    }
}
