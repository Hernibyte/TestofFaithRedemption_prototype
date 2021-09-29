using UnityEngine;

namespace Proto1
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("PLAYER NEEDS")]
        [SerializeField] Animator playerAnimator;
        [SerializeField] SpriteRenderer mySprite;
        [SerializeField] Transform attackPoint;
        PlayerAttack playerAttack;
        public Rigidbody2D rig;
        [HideInInspector] public RoomID actualRoom;
        [SerializeField] StatsMenu stats;
        [SerializeField] CardTaked  cardTakenSystem;


        [Header("PLAYER MOVE STATS")]
        [Space(10)]
        [SerializeField] bool canDodge;
        [SerializeField] bool isDoging;
        [SerializeField] bool canMove;
        [SerializeField] public float speed;
        [SerializeField] public int playerOnRoom;
        [SerializeField] public float decreaseSpeedAttack;
        [SerializeField] public float dodgeOnRecover;
        [SerializeField] public float dodgeForce;

        [SerializeField] public float playerSpeedCap;
        [SerializeField] public float playerMinSpeed;
        [SerializeField][Range(0.5f,2)] public float recoverDodge;
        [SerializeField] public float distanceBetweenImages;
        private float lastImagePosX;
        private float lastImagePosY;

        void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            playerAttack = GetComponentInChildren<PlayerAttack>();
        }

        void Start()
        {
            canDodge = true;
            isDoging = false;
            canMove = true;
            playerOnRoom = 0;
        }

        void Update()
        {
            if (dodgeOnRecover <= recoverDodge)
            {
                dodgeOnRecover += Time.deltaTime;
            }

            if(isDoging)
            {
                if (Mathf.Abs(transform.position.x - lastImagePosX) > distanceBetweenImages ||
                    Mathf.Abs(transform.position.y - lastImagePosY) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImagePosX = transform.position.x;
                    lastImagePosY = transform.position.y;
                }
            }

            if(dodgeOnRecover > recoverDodge)
            {
                dodgeOnRecover = recoverDodge;
            }

            if(dodgeOnRecover >= recoverDodge / 2)
                canMove = true;
            if (dodgeOnRecover == recoverDodge)
            {
                canDodge = true;
                isDoging = false;
            }
        }

        private void FixedUpdate()
        {
            Movement();
        }

        public RoomID GetPlayerOnRoom()
        {
            return actualRoom;
        }

        public void SetPlayerOnRoom(RoomID room)
        {
            if (room == null)
                return;

            actualRoom = room;
            playerOnRoom = room.roomId;
        }

        void DodgeInDirection(Vector2 direction)
        {
            if (!canDodge)
                return;

            if (Input.GetKey(KeyCode.Space) && rig.velocity != Vector2.zero)
            {
                rig.AddForce(direction.normalized * dodgeForce, ForceMode2D.Impulse);
                playerAnimator.SetTrigger("dodge");

                dodgeOnRecover = 0;
                canDodge = false;
                canMove = false;
                isDoging = true;

                PlayerAfterImagePool.Instance.GetFromPool();
                lastImagePosX = transform.position.x;
                lastImagePosY = transform.position.y;
            }
        }

        void Movement()
        {
            if (stats.openStats)
            {
                playerAnimator.SetFloat("speed", 0);
                return;
            }
            if(cardTakenSystem.isOpen)
                return;
            if (!canMove)
                return;

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector2 position = new Vector2(x * speed, y * speed);

            rig.AddForce(position);

            DodgeInDirection(position);

            if (playerAttack.attackColdownMelee > playerAttack.attackSpeedMelee - decreaseSpeedAttack)
                rig.velocity = Vector3.zero;

            if (playerAttack.attackColdownRanged > playerAttack.attackSpeedRanged - decreaseSpeedAttack * 0.5f)
                rig.velocity = Vector3.zero;

            if (playerAnimator != null)
            {
                if(attackPoint.position.x > transform.position.x)
                    mySprite.flipX = false;
                else
                    mySprite.flipX = true;

                if (position != Vector2.zero)
                {
                    playerAnimator.SetFloat("speed", position.magnitude);
                }
                else
                {
                    playerAnimator.SetFloat("speed", 0);
                }
            }
        }
    }
}
