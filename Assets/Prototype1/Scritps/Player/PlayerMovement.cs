using UnityEngine;

namespace Proto1
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float speed;
        PlayerAttack playerAttack;
        Rigidbody2D rig;
        RoomBehaviour actualRoom;
        [SerializeField] int playerOnRoom;

        void Awake() 
        {
            rig = GetComponent<Rigidbody2D>();
            playerAttack = GetComponent<PlayerAttack>();
        }

        void Start()
        {
            playerOnRoom = 0;
        }

        void Update()
        {
            Movement();
        }

        public RoomBehaviour GetPlayerOnRoom()
        {
            return actualRoom;
        }

        void SetPlayerOnRoom(RoomBehaviour room)
        {
            if (room == null)
                return;

            actualRoom = room;
            playerOnRoom = room.roomId;
        }

        void Movement()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector2 position = new Vector2(x * Time.deltaTime * speed, y * Time.deltaTime * speed);

            rig.AddForce(position);

            if (x > 0)
            {
                playerAttack.horizontalAttack = 0.4f;
                playerAttack.verticalAttack = 0;
            }
            else if (x < 0)
            {
                playerAttack.horizontalAttack = -0.4f;
                playerAttack.verticalAttack = 0;
            }

            if (y > 0)
            {
                playerAttack.verticalAttack = 0.4f;
                playerAttack.horizontalAttack = 0;
            }
            else if (y < 0)
            {
                playerAttack.verticalAttack = -0.4f;
                playerAttack.horizontalAttack = 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("RoomSpace"))
            {
                actualRoom = collision.GetComponent<RoomBehaviour>();
                SetPlayerOnRoom(actualRoom);
            }
        }
    }
}
