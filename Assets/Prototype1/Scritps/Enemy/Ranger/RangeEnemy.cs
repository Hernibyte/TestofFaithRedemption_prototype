using UnityEngine;
using UnityEngine.Events;


namespace Proto1
{
    public class RangeEnemy : MonoBehaviour, IHittable
    {
        [SerializeField] LayerMask playerLayer;
        [SerializeField] Animator enemyAnimator;
        [SerializeField] SpriteRenderer spriteEnemy;
        [SerializeField] Rigidbody2D rig;
        [SerializeField] float enemySpeed;

        [SerializeField] GameObject target;

        [SerializeField] public float enemyHP;
        [SerializeField] public float enemyMaxHP;
        [SerializeField] public int enemyDamage;
        [SerializeField] float enemyKnockBackForce;
        [SerializeField] float distanceAttack;
        [SerializeField] float distanceToAgro;

        [SerializeField] RangerBullet prefabBullet;
        [SerializeField] Transform firePoint;
        [SerializeField] float deleayPerShoot;
        [SerializeField] float speedBullet;
        float timerPerShoot;
        public Vector2 directionAttack;
        public Vector2 newPosWhereMove;

        [Space(20)]

        private bool movingToNewPosition;
        private float timeMoving;
        private float movingOtherPosition;

        public delegate void UpdateEnemyUIData(int amountDamage);
        public UpdateEnemyUIData updateUIData;

        PlayerAttack playerReference;
        public UnityEvent deathEvent;

        [System.Serializable]
        public enum STATENEMY
        {
            Idle,
            Attacking,
            FallBack,
            BeignDamaged
        }
        public STATENEMY enemyState;

        void Start()
        {
            timerPerShoot = 0;

            target = GameObject.FindGameObjectWithTag("Player");
            playerReference = target.GetComponent<PlayerAttack>();

            if(playerReference != null)
                playerReference.attackFromPlayer += ChangePosition;

            timeMoving = 0;
            movingOtherPosition = 2f;
            movingToNewPosition = false;

            enemyMaxHP = enemyHP;
            enemyState = STATENEMY.Idle;
        }
        private void OnDisable()
        {
            if(playerReference != null)
                playerReference.attackFromPlayer -= ChangePosition;
        }
        void Update()
        {
            if (target == null)
                return;

            switch (enemyState)     
            {
                case STATENEMY.Idle:

                    if(Vector2.Distance(rig.position, target.transform.position) < distanceToAgro)
                    {
                        enemyState = STATENEMY.Attacking;
                    }

                    break;
                case STATENEMY.Attacking:

                    directionAttack = new Vector2(target.transform.position.x, target.transform.position.y) - rig.position;

                    Ray2D rayoInterdimensionalePuro = new Ray2D(rig.position, directionAttack.normalized);
                    Debug.DrawRay(rayoInterdimensionalePuro.origin, rayoInterdimensionalePuro.direction * distanceAttack, Color.red);

                    if (timerPerShoot > 0)
                        timerPerShoot -= Time.deltaTime;
                    else
                    {
                        timerPerShoot = 0;
                    }

                    if (timerPerShoot > 0)
                    {
                        enemyAnimator.SetBool("attack", false);
                        return;
                    }

                    RangerBullet bullet = Instantiate(prefabBullet, firePoint.position, Quaternion.identity);
                    enemyAnimator.SetBool("attack", true);
                    if(bullet != null)
                    {
                        bullet.FixRotation(directionAttack);
                        bullet.SetBulletParams(enemyDamage, enemyKnockBackForce);

                        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

                        if(bulletRb != null)
                            bulletRb.AddForce(rayoInterdimensionalePuro.direction * speedBullet * Time.deltaTime, ForceMode2D.Impulse);

                        timerPerShoot = deleayPerShoot;
                    }
                    break;
                case STATENEMY.FallBack:

                    if (timeMoving < movingOtherPosition)
                        timeMoving += Time.deltaTime;
                    else
                        timeMoving = movingOtherPosition;


                    if (timeMoving >= movingOtherPosition)
                    {
                        enemyAnimator.SetBool("fallBack", false);
                        enemyState = STATENEMY.Attacking;
                        rig.velocity = Vector2.zero;
                        movingToNewPosition = false;
                    }

                    break;
                case STATENEMY.BeignDamaged:


                    break;
            }
        }

        public void ChangePosition()
        {
            int randomProb = Random.Range(0, 100);

            if (randomProb < 60)
                return;
            else
            {
                if (movingToNewPosition)
                    return;

                movingToNewPosition = true;

                enemyAnimator.SetBool("attack", false);
                enemyAnimator.SetBool("fallBack", true);
                timeMoving = 0;

                int direction = Random.Range(0,100);
                if(direction < 25)
                    rig.AddForce(rig.transform.right * (enemySpeed * 20) * Time.deltaTime, ForceMode2D.Impulse);
                else if(direction > 25 && direction < 50)
                    rig.AddForce(-rig.transform.right * (enemySpeed * 20) * Time.deltaTime, ForceMode2D.Impulse);
                else if(direction > 50 && direction < 75)
                    rig.AddForce(rig.transform.up * (enemySpeed * 20) * Time.deltaTime, ForceMode2D.Impulse);
                else if(direction > 75 && direction < 100)
                    rig.AddForce(-rig.transform.up * (enemySpeed * 20) * Time.deltaTime, ForceMode2D.Impulse);

                enemyState = STATENEMY.FallBack;
                timerPerShoot = deleayPerShoot;
            }
        }

        public void Hit(int amountDamage, float knockBackForce, Vector2 posAttacker)
        {
            if (enemyHP > 0)
            {
                enemyHP -= amountDamage;

                updateUIData?.Invoke(amountDamage);
            }
            else
            {
                enemyHP = 0;
            }

            if (enemyHP <= 0)
            {
                Die();
            }
        }
        public void Die()
        {
            deathEvent?.Invoke();
            Destroy(gameObject);
        }
    }
}