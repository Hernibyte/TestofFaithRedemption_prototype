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

    [SerializeField] public List<GameObject> roomList;
    public float localRoomTime;
    [SerializeField] GameObject bossObject;
    GameObject UIType_Boss;
    bool enemySpawned;

    private void Awake()
    {
        UIType_Boss = FindObjectOfType<Proto1.UI_Boss>().gameObject;
        UIType_Boss.SetActive(false);
    }

    private void Start()
    {
        localRoomTime = 2f;
    }

    private void Update()
    {
        if (!enemySpawned)
        {
            localRoomTime -= Time.deltaTime;

            if (localRoomTime <= 0f)
            {
                int index = roomList.Count - 1;
                Vector3 bossPosition = roomList[index].transform.position;

                if (roomList[index].GetComponentInChildren<EnemyGenerator>())
                    roomList[index].GetComponentInChildren<EnemyGenerator>().imSpawnBoss = true;

                Instantiate(bossObject, bossPosition, Quaternion.identity);

                UIType_Boss.SetActive(true);

                if (roomList[index].GetComponentInChildren<EnemyGenerator>())
                    roomList[index].GetComponentInChildren<EnemyGenerator>().FindUI_Boss();

                enemySpawned = true;
            }
        }
    }
}
