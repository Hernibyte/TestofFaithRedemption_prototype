using System.Collections.Generic;
using UnityEngine;

public class RoomState : MonoBehaviour
{
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public bool isLocked;
    [SerializeField] public EnemyGenerator enemiesInside;
    [SerializeField] public List<Door> doors;

    public bool allDoorsOpen = false;
    public bool allDoorsClosed = false;


    float timeToClose = 0;
    float timerToSetBool = 1;
    void Update()
    {
        if(isLocked)
        {
            if (timeToClose < timerToSetBool)
                timeToClose += Time.deltaTime;
            else
            {
                timeToClose = timerToSetBool;
                allDoorsOpen = false;
            }
        }


        if(enemiesInside.enemiesAlive <= 0)
        {
            if (!allDoorsOpen)
            {
                for (int i = 0; i < doors.Count; i++)
                {
                    if(doors[i] != null)
                        doors[i].OpenDoor();
                }
                allDoorsOpen = true;
                allDoorsClosed = false;
                isLocked = false;
            }
        }
        
    }
    bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timeToClose >= timerToSetBool)
            return;

        if(Contains(playerLayer, collision.gameObject.layer))
        {
            if(!allDoorsClosed)
            {
                for (int i = 0; i < doors.Count; i++)
                {
                    if(doors[i] != null)
                        doors[i].CloseDoor();
                }
                allDoorsClosed = true;
                isLocked = true;
            }
        }
    }
}
