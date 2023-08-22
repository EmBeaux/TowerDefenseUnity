using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner
{
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public float spawnRate;
    public int enemyCount;
    private float spawnDelay = 2f;
    private bool gameStart = false;

    public EnemySpawner(float spawnRate, Transform enemyPrefab, int enemyCount, Transform spawnPoint)
    {
        this.spawnRate = spawnRate;
        this.enemyPrefab = enemyPrefab;
        this.spawnPoint = spawnPoint;
        this.enemyCount = enemyCount;
    }

    public void Tick()
    {
        if (gameStart)
        {
            if (spawnDelay <= 0f && enemyCount > 0)
            {
                SpawnEnemy();
                spawnDelay = 1f / spawnRate;
                enemyCount--;
            }

            spawnDelay -= Time.deltaTime;
        }
    }

    public void StartGame()
    {
        gameStart = true;
    }

    private void SpawnEnemy()
    {
        Transform enemy = GameObject.Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.parent = spawnPoint;
    }
}