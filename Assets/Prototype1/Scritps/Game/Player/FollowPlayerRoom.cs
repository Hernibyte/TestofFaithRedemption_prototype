using UnityEngine;

namespace Proto1
{
    public class FollowPlayerRoom : MonoBehaviour
    {
        [SerializeField] PlayerMovement playerRef;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("RoomSpace"))
            {
                if(playerRef != null)
                {
                    playerRef.actualRoom = collision.GetComponent<RoomID>();
                    playerRef.SetPlayerOnRoom(playerRef.actualRoom);
                }
            }
        }
    }
}