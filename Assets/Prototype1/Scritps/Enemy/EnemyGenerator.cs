using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject prefabEnemyWarrior;
    [SerializeField] GameObject prefabEnemyRanger;
    [SerializeField] GameObject spawnArea;
    [SerializeField] bool enemyCreated;
    [SerializeField] GameObject lootChest;

    [SerializeField] List<GameObject> spawnPoints;

    private void Start()
    {
        enemyCreated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!enemyCreated)
        {
            int randomSpawner = Random.Range(0, spawnPoints.Count-1);
            GameObject obj = Instantiate(prefabEnemyWarrior, spawnPoints[randomSpawner].transform.position, Quaternion.identity);
            
            randomSpawner = Random.Range(0, spawnPoints.Count-1);
            Instantiate(prefabEnemyRanger, spawnPoints[randomSpawner].transform.position, Quaternion.identity);
            
            enemyCreated = true;
            obj.GetComponent<Proto1.MeleeEnemy>().deathEvent.AddListener(SpawnLoot);
        }
    }

    void SpawnLoot()
    {
        Instantiate(lootChest, spawnArea.transform.position, Quaternion.identity);
    }
}
