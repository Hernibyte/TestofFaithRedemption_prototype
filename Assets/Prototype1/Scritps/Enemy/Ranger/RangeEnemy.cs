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

        [SerializeField] float enemyHP;
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

        [SerializeField] bool hasTakenDamage;

        [Space(20)]

        private float timeToRestore;
        private float coldownAfterHit;

        public delegate void UpdateEnemyUIData(int amountDamage);
        public UpdateEnemyUIData updateUIData;

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
            hasTakenDamage = false;

            timeToRestore = 0;
            coldownAfterHit = 0.1f;
            enemyMaxHP = enemyHP;
            enemyState = STATENEMY.Idle;
            target = GameObject.FindGameObjectWithTag("Player");

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

                    if (hasTakenDamage)
                    {
                        enemyAnimator.SetBool("attack", false);
                        enemyAnimator.SetBool("fallBack", true);
                        enemyState = STATENEMY.FallBack;
                        timerPerShoot = deleayPerShoot;
                        newPosWhereMove = target.transform.position * -1;
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

                    rig.MovePosition(Vector2.MoveTowards(rig.position, newPosWhereMove, enemySpeed * Time.deltaTime));
                    
                    if(rig.position == newPosWhereMove)
                    {
                        enemyAnimator.SetBool("fallBack", false);
                        enemyState = STATENEMY.Attacking;
                        hasTakenDamage = false;
                    }

                    break;
                case STATENEMY.BeignDamaged:


                    break;
            }
        }
        public void Hit(int amountDamage, float knockBackForce, Vector2 posAttacker)
        {
            if (enemyHP > 0)
            {
                enemyHP -= amountDamage;

                hasTakenDamage = true;

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
            Destroy(gameObject);
        }
    }
}