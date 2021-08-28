using UnityEngine;

public class RoomID : MonoBehaviour
{
    [SerializeField] public int roomId;

    public void SetID(int id)
    {
        roomId = id;
    }
}
