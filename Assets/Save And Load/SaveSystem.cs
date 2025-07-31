using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static readonly string path = Application.persistentDataPath + "/playerdata.dat";

    public static void SavePlayer(PlayerHealth player, Ammo ammo, GunPickUpStateManager gunState, FlashlightStateManager flashlightState, StartingSequence startingSequence, BookPickUpManager bookPickUpManager)
    {
        try
        {
            PlayerData data = new PlayerData(player, ammo, gunState, flashlightState, startingSequence, bookPickUpManager);

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to save player data: " + e.Message);
        }
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(path))
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(stream) as PlayerData;
                }
            }
            catch (IOException e)
            {
                Debug.LogError("Failed to load player data: " + e.Message);
                return null;
            }
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
    }

    public static void DeletePlayerData()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted at: " + path);
        }
        else
        {
            Debug.LogWarning("No save file to delete at: " + path);
        }
    }
}
