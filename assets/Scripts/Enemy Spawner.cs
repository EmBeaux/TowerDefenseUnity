using System.Collections.Generic;
using UnityEngine;

public struct WaveDetails {
    public float spawnRate;
    public int enemyCount;
    public Transform spawnPoint;
    public Transform enemyPrefab;

    public WaveDetails(float spawnRate, int enemyCount, Transform spawnPoint, Transform enemyPrefab)
    {
        this.spawnRate = spawnRate;
        this.enemyCount = enemyCount;
        this.spawnPoint = spawnPoint;
        this.enemyPrefab = enemyPrefab;
    }
}

public class EnemySpawner
{
    public WaveDetails waveDetails;
    private float spawnDelay = 2f;
    private bool gameStart = false;
    private bool isFirstTick = true;

    public EnemySpawner(WaveDetails waveDetails)
    {
        this.waveDetails = waveDetails;
    }

    public void Tick()
    {
        if (gameStart)
        {
            if (isFirstTick || spawnDelay <= 0f && waveDetails.enemyCount > 0)
            {
                SpawnEnemy();
                spawnDelay = 1f / waveDetails.spawnRate;
                waveDetails.enemyCount--;
                isFirstTick = false;
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
        Transform enemy = GameObject.Instantiate(
            waveDetails.enemyPrefab,
            waveDetails.spawnPoint.position,
            waveDetails.spawnPoint.rotation
        );
        enemy.parent = waveDetails.spawnPoint;
    }
}