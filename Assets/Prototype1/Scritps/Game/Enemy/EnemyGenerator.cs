using System.Collections.Generic;
using UnityEngine;
using Proto1;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] WarriorEnemy prefabEnemyWarrior;
    [SerializeField] RangeEnemy prefabEnemyRanger;
    [SerializeField] GameObject spawnArea;
    [SerializeField] bool enemyCreated;
    [SerializeField] GameObject lootChest;
    [Header("Enemies Amount")]
    [SerializeField] int ragnersEnemyAmount;
    [SerializeField] int meleeEnemyAmount;
    [Header("only 0 or 1")]
    [SerializeField] int rangersMinAmount;
    [SerializeField] int meleeMinAmount;
    int rangersInRoom;
    int meleeEnemiesInRoom;
    [Header("")]

    [SerializeField] public int enemiesAlive;

    [SerializeField] List<GameObject> spawnPoints;

    //[SerializeField] MeleeEnemy warriorEnemy; //Lpm con no poder crear mas de uno hay que cambiar el loot system xd | igual es innecesario pa
    //[SerializeField] RangeEnemy[] rangedEnemies; // Esto que hiciste es mas rancio que todo mi codigo junto facu lpm

    public bool imSpawnBoss;
    public UI_Boss uiBoss;

    [Space(20)]
    [SerializeField] public RoomID checkEmptyRooms;

    private void Start()
    {

        enemyCreated = false;
        //rangedEnemies = new RangeEnemy[rangersInRoom]; // Porque!!
    }

    void DecreaseEnemiesAliveRoom()
    {
        enemiesAlive--;
        if(enemiesAlive <= 0)
        {
            SpawnLoot();
        }
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
            meleeEnemiesInRoom = Random.Range(meleeMinAmount, meleeEnemyAmount+1);
            rangersInRoom = Random.Range(rangersMinAmount, ragnersEnemyAmount+1);
            //
            int randomSpawner = 0;
            for(int i = 0; i < meleeEnemiesInRoom; i++)
            {
                randomSpawner = Random.Range(0, spawnPoints.Count);
                WarriorEnemy obj = Instantiate(prefabEnemyWarrior, spawnPoints[randomSpawner].transform.position, Quaternion.identity);
                obj.deathEvent.AddListener(DecreaseEnemiesAliveRoom);
                enemiesAlive++;
            }
            for (int i = 0; i < rangersInRoom; i++)
            {
                randomSpawner = Random.Range(0, spawnPoints.Count);
                //rangedEnemies[i] = Instantiate(prefabEnemyRanger, spawnPoints[randomSpawner].transform.position, Quaternion.identity); // Que es esto!!
                //rangedEnemies[i].deathEvent.AddListener(DecreaseEnemiesAliveRoom); Como vas a poner esto asi forro jajajaj
                RangeEnemy obj = Instantiate(prefabEnemyRanger, spawnPoints[randomSpawner].transform.position, Quaternion.identity);
                obj.deathEvent.AddListener(DecreaseEnemiesAliveRoom);
                enemiesAlive++;
            }
            enemyCreated = true;
        }
    }

    void SpawnLoot()
    {
        int rand = Random.Range(0, 2);
        switch(rand)
        {
            case 1:
                Instantiate(lootChest, spawnArea.transform.position, Quaternion.identity);
                break;
            default:
                //nada jaja salu2 
                //JAJAJAJA dios mio este piB
                break;
        }
    }

    public void FindUI_Boss()
    {
        uiBoss = FindObjectOfType<UI_Boss>();
    }
}
