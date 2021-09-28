using UnityEngine;

public class RoomID : MonoBehaviour
{
    [SerializeField] public int roomId;

    [SerializeField] float distanceBetweenRooms;
    [SerializeField] LayerMask roomLayer;
    [SerializeField] GameObject[] doors;

    public enum DirectionDoors
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public DirectionDoors [] directionDoors;

    public bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public void CheckAllDirections()
    {
        for (int i = 0; i < directionDoors.Length; i++)
        {
            switch (directionDoors[i])
            {
                case DirectionDoors.UP:
                    Ray2D upDirection = new Ray2D(gameObject.transform.position, Vector2.up);

                    Debug.DrawRay(upDirection.origin,upDirection.direction * distanceBetweenRooms, Color.red);

                    RaycastDirection(ref upDirection, i);

                    break;
                case DirectionDoors.DOWN:

                    Ray2D downDirection = new Ray2D(gameObject.transform.position, Vector2.down);

                    Debug.DrawRay(downDirection.origin, downDirection.direction * distanceBetweenRooms, Color.green);

                    RaycastDirection(ref downDirection, i);

                    break;
                case DirectionDoors.LEFT:

                    Ray2D leftDirection = new Ray2D(gameObject.transform.position, Vector2.left);

                    Debug.DrawRay(leftDirection.origin, leftDirection.direction * distanceBetweenRooms, Color.yellow);

                    RaycastDirection(ref leftDirection, i);

                    break;
                case DirectionDoors.RIGHT:

                    Ray2D rightDirection = new Ray2D(gameObject.transform.position, Vector2.right);

                    Debug.DrawRay(rightDirection.origin, rightDirection.direction * distanceBetweenRooms, Color.magenta);

                    RaycastDirection(ref rightDirection, i);

                    break;
            }
        }
    }

    void RaycastDirection(ref Ray2D directionRay, int iteration)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(directionRay.origin, directionRay.direction, distanceBetweenRooms);
        
        if(hits.Length <= 1)
        {
            Collider2D colDoor = doors[iteration].gameObject.GetComponent<Collider2D>();
            Door actualDoor = doors[iteration].gameObject.GetComponentInChildren<Door>();
            if(actualDoor != null)
                actualDoor.CloseDoor();
            colDoor.isTrigger = false;
        }
        else
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject != gameObject)
                {
                    if(Contains(roomLayer, hit.transform.gameObject.layer))
                    {
                        Collider2D colDoor = doors[iteration].gameObject.GetComponent<Collider2D>();
                        colDoor.isTrigger = true;
                    }
                }
            }
        }
    }

    public void SetID(int id)
    {
        roomId = id;
    }
}
