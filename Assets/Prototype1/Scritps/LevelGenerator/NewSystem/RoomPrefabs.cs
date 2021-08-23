using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrefabs : MonoBehaviour
{
    [SerializeField] public GameObject[] topRooms;
    [SerializeField] public GameObject[] bottomRooms;
    [SerializeField] public GameObject[] rightRooms;
    [SerializeField] public GameObject[] leftRooms;

    public GameObject closedRoom;

    [SerializeField] public int amountRooms;
}
