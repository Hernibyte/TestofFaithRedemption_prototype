using UnityEngine;

public class RoomLogic : MonoBehaviour
{
    [SerializeField] int openSide;

    //1 Se necesita una puerta abajo
    //2 Se necesita una puerta arriba
    //3 Se necesita una puerta derecha
    //4 Se necesita una puerta izquierda

    private RoomPrefabs templates;
    [SerializeField] private int rand;
    [SerializeField] private bool spawned = false;
    [SerializeField] RoomID myId;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomPrefabs>();
        Invoke("Spawn", 0.4f);
    }

    void Spawn()
    {
        if (!spawned)
        {
            if (openSide == 1)
            {
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }
            else if (openSide == 2)
            {
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            else if (openSide == 3)
            {
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }
            else if (openSide == 4)
            {
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            templates.amountRooms++;

            if (myId != null)
                myId.roomId = templates.amountRooms;

            spawned = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            Destroy(gameObject);
            //if (other.GetComponent<RoomLogic>() != null)
            //{
            //    if (other.GetComponent<RoomLogic>().spawned == false && spawned == false)
            //    {
            //        if (templates != null)
            //        {
            //            Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
            //            Destroy(gameObject);
            //        }
            //    }
            //    spawned = true;
            //}
        }
    }
}
