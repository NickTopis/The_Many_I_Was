using UnityEngine;

public class SaveCube : MonoBehaviour
{
    [SerializeField] SaveAndLoadFunctions saveAndLoadFunctions;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        saveAndLoadFunctions.SaveGame();
        Debug.Log("CheckPoint");
    }
}
