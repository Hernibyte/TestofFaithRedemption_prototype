using System.Collections.Generic;
using UnityEngine;
using Proto1;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] MeleeEnemy prefabEnemyWarrior;
    [SerializeField] RangeEnemy prefabEnemyRanger;
    [SerializeField] GameObject spawnArea;
    [SerializeField] bool enemyCreated;
    [SerializeField] GameObject lootChest;

    [SerializeField] public int rangersInRoom;

    [SerializeField] public int enemiesAlive;

    [SerializeField] List<GameObject> spawnPoints;

    [SerializeField] MeleeEnemy warriorEnemy; //Lpm con no poder crear mas de uno hay que cambiar el loot system xd
    [SerializeField] RangeEnemy[] rangedEnemies;

    public bool imSpawnBoss;
    public UI_Boss uiBoss;

    private void Start()
    {
        enemyCreated = false;
        rangedEnemies = new RangeEnemy[rangersInRoom];
    }

    void DecreaseEnemiesAliveRoom()
    {
        enemiesAlive--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (imSpawnBoss)
        {
            uiBoss.bossIsActive = true;
            enemyCreated = true;
            imSpawnBoss = false;
        }
        if(!enemyCreated)
        {
            int randomSpawner = Random.Range(0, spawnPoints.Count-1);
            MeleeEnemy obj = Instantiate(prefabEnemyWarrior, spawnPoints[randomSpawner].transform.position, Quaternion.identity);
            warriorEnemy = obj;
            warriorEnemy.deathEvent.AddListener(DecreaseEnemiesAliveRoom);

            enemiesAlive++;
            for (int i = 0; i < rangersInRoom; i++)
            {
                randomSpawner = Random.Range(0, spawnPoints.Count-1);
                rangedEnemies[i] = Instantiate(prefabEnemyRanger, spawnPoints[randomSpawner].transform.position, Quaternion.identity);
                rangedEnemies[i].deathEvent.AddListener(DecreaseEnemiesAliveRoom);
                enemiesAlive++;
            }
            enemyCreated = true;
            obj.deathEvent.AddListener(SpawnLoot);
        }
    }

    void SpawnLoot()
    {
        Instantiate(lootChest, spawnArea.transform.position, Quaternion.identity);
    }

    public void FindUI_Boss()
    {
        uiBoss = FindObjectOfType<UI_Boss>();
    }
}
