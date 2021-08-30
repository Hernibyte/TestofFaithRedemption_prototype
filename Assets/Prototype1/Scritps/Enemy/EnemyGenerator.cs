using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject prefabEnemy;
    [SerializeField] GameObject spawnArea;
    [SerializeField] bool enemyCreated;
    [SerializeField] GameObject lootChest;

    private void Start()
    {
        enemyCreated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!enemyCreated)
        {
            GameObject obj = Instantiate(prefabEnemy, spawnArea.transform.position, Quaternion.identity);
            enemyCreated = true;
            obj.GetComponent<Proto1.MeleeEnemy>().deathEvent.AddListener(SpawnLoot);
        }
    }

    void SpawnLoot()
    {
        Instantiate(lootChest, spawnArea.transform.position, Quaternion.identity);
    }
}
