using UnityEngine;

namespace Proto1
{
    public class CameraTracking : MonoBehaviour
    {
        [SerializeField] PlayerMovement playerRef;
        RoomBehaviour refRoom;

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
                Debug.Log("Tengo ref player");
                
                if(refRoom != null)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(refRoom.transform.position.x, refRoom.transform.position.y,transform.position.z), 10 * Time.deltaTime);
                    Debug.Log("Entro");
                }
            }
        }
    }
}
