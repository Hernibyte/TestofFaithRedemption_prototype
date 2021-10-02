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

    public delegate void ActivateGameplayAfterDungeon();
    public static ActivateGameplayAfterDungeon activateGameplay;

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
                ClearDuplicateRoom();

                int index = roomList.Count - 1;
                Vector3 bossPosition = roomList[index].transform.position;

                if (roomList[index].GetComponentInChildren<EnemyGenerator>())
                    roomList[index].GetComponentInChildren<EnemyGenerator>().imSpawnBoss = true;

                EnemyGenerator roomEnemyGenRef = roomList[index].GetComponentInChildren<EnemyGenerator>();
                GameObject bossGo = Instantiate(bossObject, bossPosition, Quaternion.identity);

                Proto1.BossEnemy bossEnemy= bossGo.GetComponent<Proto1.BossEnemy>();
                if (bossEnemy != null)
                {
                    if(roomEnemyGenRef != null)
                        bossEnemy.SetEnemyGenerator(roomEnemyGenRef);
                }

                UIType_Boss.SetActive(true);

                if (roomList[index].GetComponentInChildren<EnemyGenerator>())
                {
                    EnemyGenerator boosRoom = roomList[index].GetComponentInChildren<EnemyGenerator>();
                    if(boosRoom != null)
                    {
                        CloseEmptyRooms();
                        boosRoom.FindUI_Boss();
                        boosRoom.enemiesAlive = 1;
                        activateGameplay?.Invoke();
                    }
                }

                enemySpawned = true;
            }
        }
    }

    void CloseEmptyRooms()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomID idRoom = roomList[i].GetComponent<RoomID>();

            if(idRoom != null)
            {
                idRoom.CheckAllDirections();
            }
        }

    }

    void ClearDuplicateRoom()
    {
        List<int> index = new List<int>();
        GameObject objeto = roomList[0];
        for (int i = 1; i < roomList.Count; i++)
        {
            if (roomList[i].transform.position == objeto.transform.position)
            {
                index.Add(i);
            }
            objeto = roomList[i];
        }
        for (int i = 0; i < index.Count; i++)
        {
            roomList.RemoveAt(index[i]);
        }
    }
}
