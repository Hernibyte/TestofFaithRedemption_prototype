using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class RoomGenerator : MonoBehaviour
    {
        [SerializeField] List<GameObject> rooms;
        [SerializeField] GameObject firstRoom;
        [SerializeField] int roomMaxCount;

        private void Start()
        {
            Instantiate(firstRoom, new Vector3(0f, 0f, 0f), Quaternion.identity, transform);
        }

        public void Generate(float x, float y, int openDoor)
        {
            while (roomMaxCount > 0)
            {
                int random = Random.Range(0, rooms.Count);
                if (rooms[random].GetComponent<RoomBehaviour>().doors[openDoor])
                {
                    GameObject room = Instantiate(rooms[random], new Vector3(x, y, 0f), Quaternion.identity, transform);
                    room.GetComponent<RoomBehaviour>().doors[openDoor] = false;
                    roomMaxCount--;
                    break;
                }
            }
        }
    }
}
