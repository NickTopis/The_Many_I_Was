using UnityEngine;

public class PlayerOnElevator : MonoBehaviour
{
    [SerializeField] Transform elevator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(elevator);  // Parent to elevator
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);       // Unparent when exiting
        }
    }
}
