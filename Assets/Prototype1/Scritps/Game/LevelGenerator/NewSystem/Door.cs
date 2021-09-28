using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator stateDoor;
    [SerializeField] Collider2D doorColl;

    public void OpenDoor()
    {
        stateDoor.SetBool("DoorOpen", true);
        doorColl.isTrigger = true;
    }
    public void CloseDoor()
    {
        stateDoor.SetBool("DoorOpen", false);
        doorColl.isTrigger = false;
    }
}
