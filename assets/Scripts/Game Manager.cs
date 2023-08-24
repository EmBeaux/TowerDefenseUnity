using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform enemyPrefab, fastEnemyPrefab;
    public Transform spawnPoint;
    public Transform endPoint;
    public List<List<EnemySpawner>> enemySpawners;
    public List<EnemySpawner> currentSpawnerBatch;
    public int totalWaves = 15;
    public int currentWave = 0;
    public EnemySpawner currentSpawner;
    public bool gameStarted = false;

    public void StartGameManager(Transform startTower, Transform endTower)
    {
        spawnPoint = startTower;
        endPoint = endTower;
        gameStarted = true;
        float prevSpawnRate = 2f;
        int prevEnemyCount = 5;
        enemySpawners = new List<List<EnemySpawner>>();
        for (int i = 0; i < totalWaves; i++)
        {
            List<EnemySpawner> currentSpawnerBatch = new List<EnemySpawner>();
            currentSpawnerBatch.Add(
                new EnemySpawner(
                    prevSpawnRate += .5f,
                    enemyPrefab,
                    prevEnemyCount += 5,
                    spawnPoint
                )
            );

            if (i > 4 && (i + 1) % 2 == 0)
            {
                int newEnemyCount = prevEnemyCount / 2;
                currentSpawnerBatch.Add(
                    new EnemySpawner(
                        prevSpawnRate * 2f,
                        fastEnemyPrefab,
                        newEnemyCount,
                        spawnPoint
                    )
                );
            }

            enemySpawners.Add(currentSpawnerBatch);
        }
    }
    private void Update()
    {
        if (gameStarted && Input.GetKeyDown(KeyCode.Space) && FindObjectsOfType<Enemy>().Length == 0)
        {
            Debug.Log("Current wave " + currentWave);
            currentSpawnerBatch = enemySpawners[currentWave];
            foreach(EnemySpawner spawner in currentSpawnerBatch)
            {
                spawner.StartGame();
            }
            currentWave++;
        }

        if (currentSpawnerBatch != null)
        {
            foreach (EnemySpawner spawner in currentSpawnerBatch)
            {
                spawner.Tick();
            }
        }
    }
}
