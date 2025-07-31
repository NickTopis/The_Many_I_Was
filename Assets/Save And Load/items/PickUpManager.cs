using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager Instance;

    private HashSet<string> pickedUpIDs = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void MarkPickedUp(string id)
    {
        pickedUpIDs.Add(id);
    }

    public bool IsPickedUp(string id)
    {
        return pickedUpIDs.Contains(id);
    }

    public List<string> GetPickedUpIDs()
    {
        return new List<string>(pickedUpIDs);
    }

    public void LoadPickedUpIDs(List<string> ids)
    {
        pickedUpIDs = new HashSet<string>(ids);
    }
}
