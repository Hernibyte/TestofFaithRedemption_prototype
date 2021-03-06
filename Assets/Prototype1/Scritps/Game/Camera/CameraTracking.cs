using UnityEngine;

namespace Proto1
{
    public class CameraTracking : MonoBehaviour
    {
        [SerializeField] PlayerMovement playerRef;
        RoomID refRoom;

        private void Start()
        {
            refRoom = null;
        }

        void Update()
        {
            TrackPlayerOnRoom();
        }

        void TrackPlayerOnRoom()
        {
            if(playerRef != null)
            {
                refRoom = playerRef.GetPlayerOnRoom();
                if(refRoom != null)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(refRoom.transform.position.x, refRoom.transform.position.y,transform.position.z), 5 * Time.deltaTime);
                }
            }
        }
    }
}
