using UnityEngine;

namespace Proto1
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("PLAYER NEEDS")]
        [SerializeField] Animator playerAnimator;
        [SerializeField] public SpriteRenderer mySprite;
        [SerializeField] Transform attackPoint;
        public PlayerAttack playerAttack;
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

        [HideInInspector] public bool activateGameplay;

        [HideInInspector] public bool godMode;

        void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            playerAttack = GetComponentInChildren<PlayerAttack>();
            activateGameplay = false;
            godMode = false;
        }

        void Start()
        {
            canDodge = true;
            isDoging = false;
            canMove = true;
            playerOnRoom = 0;

            RoomPrefabs.activateGameplay += ActivateGameplay;
        }

        void Update()
        {
            if (!activateGameplay)
                return;

            if (dodgeOnRecover <= recoverDodge)
            {
                dodgeOnRecover += Time.deltaTime;
            }

            if(isDoging)
            {
                DoAfterImageEffect();
            }
            else if(godMode)
            {
                DoAfterImageEffect();
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
            if (!activateGameplay)
                return;

            Movement();
        }

        private void OnDisable()
        {
            RoomPrefabs.activateGameplay -= ActivateGameplay;
        }

        public void DoAfterImageEffect()
        {
            if (Mathf.Abs(transform.position.x - lastImagePosX) > distanceBetweenImages ||
                    Mathf.Abs(transform.position.y - lastImagePosY) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImagePosX = transform.position.x;
                lastImagePosY = transform.position.y;
            }
        }

        public void ActivateGameplay()
        {
            activateGameplay = true;
            playerAnimator.SetBool("StartGame", activateGameplay);
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

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Vector2 direction = new Vector2(x,y);
            if (direction.magnitude > 1)
                direction.Normalize();

            Vector2 position = new Vector2(direction.x * speed, direction.y * speed);

            rig.AddForce(position);

            DodgeInDirection(position);

            if (playerAttack.attackColdownRanged > playerAttack.attackSpeedRanged - decreaseSpeedAttack * 0.5f)
                rig.velocity = Vector3.zero;

            if(attackPoint.position.x > transform.position.x)
                mySprite.flipX = false;
            else
                mySprite.flipX = true;
            
            if (playerAnimator != null)
            {
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
