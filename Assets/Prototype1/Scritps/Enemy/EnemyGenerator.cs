﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject prefabEnemy;
    [SerializeField] GameObject spawnArea;
    [SerializeField] bool enemyCreated;

    private void Start()
    {
        enemyCreated = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!enemyCreated)
        {
            Instantiate(prefabEnemy, spawnArea.transform.position, Quaternion.identity);
            enemyCreated = true;
        }
    }
}
