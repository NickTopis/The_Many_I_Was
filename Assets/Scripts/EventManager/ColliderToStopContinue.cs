using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderToStopContinue : MonoBehaviour
{
    [SerializeField] Pausemenu pausemenu;
    [SerializeField] SaveAndLoadFunctions saveAndLoadFunctions;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            saveAndLoadFunctions.DeleteSaveData();
            pausemenu.LoadMenu();
        }
    }
}
