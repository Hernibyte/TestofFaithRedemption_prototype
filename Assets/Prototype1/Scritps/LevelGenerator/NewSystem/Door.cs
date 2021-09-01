using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animation stateDoor;
    [SerializeField] string openDoor;
    [SerializeField] string closeDoor;

    public void OpenDoor()
    {
        stateDoor.Play(openDoor);
    }
    public void CloseDoor()
    {
        stateDoor.Play(closeDoor);
    }
}
