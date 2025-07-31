using UnityEngine;

public class ComponentsController : MonoBehaviour
{
    [SerializeField] ShaftLeverControll leverControll;
    [SerializeField] Collider leverCollider;
    [SerializeField] Collider door1Collider;
    [SerializeField] Collider door2Collider;
    [SerializeField] Collider blockCollider;

    // sets the down true or false , set isMoving to false and enables door controll after the animation has finished playing

    public void EnableDoors()
    {
        leverCollider.enabled = true;
        leverControll.isMoving = false;
        door1Collider.enabled = true;
        door2Collider.enabled = true;
        blockCollider.enabled = false;
    }

    public void DisableDoors()
    {
        leverCollider.enabled = false;
        door1Collider.enabled = false;
        door2Collider.enabled = false;
        blockCollider.enabled = true;
    }
}
