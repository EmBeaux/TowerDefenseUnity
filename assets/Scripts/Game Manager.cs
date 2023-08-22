using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public Transform endPoint;
    public List<EnemySpawner> enemySpawners;
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
        enemySpawners = new List<EnemySpawner>();
        for (int i = 0; i < totalWaves; i++)
        {
            enemySpawners.Add(
                new EnemySpawner(
                    prevSpawnRate += .5f,
                    enemyPrefab,
                    prevEnemyCount += 5,
                    spawnPoint
                )
            );
        }
    }
    private void Update()
    {
        if (gameStarted && Input.GetKeyDown(KeyCode.Space) && FindObjectsOfType<Enemy>().Length == 0)
        {
            Debug.Log("Current wave " + currentWave);
            currentSpawner = enemySpawners[currentWave];
            currentSpawner.StartGame();
            currentWave++;
        }

        if (currentSpawner != null)
        {
            currentSpawner.Tick();
        }
    }
}
