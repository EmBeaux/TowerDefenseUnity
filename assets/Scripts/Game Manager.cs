using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform enemyPrefab, fastEnemyPrefab, spawnPoint, endPoint;
    public List<List<EnemySpawner>> enemySpawners;
    public List<EnemySpawner> currentSpawnerBatch;
    public int totalWaves = 15, currentWave = 0;
    private bool gameStarted = false;
    private InfoManager infoManager;

    public void StartGameManager(Transform inputSpawnPoint, Transform inputEndPoint)
    {
        this.spawnPoint = inputSpawnPoint;
        this.endPoint = inputEndPoint;
        gameStarted = true;

        InitializeEnemySpawners();
    }

    private void InitializeEnemySpawners()
    {
        float spawnRateIncrement = 0.5f;
        int enemyCountIncrement = 5;
        float prevSpawnRate = 2f;
        int prevEnemyCount = 5;

        enemySpawners = new List<List<EnemySpawner>>();

        for (int i = 0; i < totalWaves; i++)
        {
            List<EnemySpawner> batch = new List<EnemySpawner>();

            batch.Add(CreateEnemySpawner(prevSpawnRate, prevEnemyCount, enemyPrefab));

            if (i > 4)
            {
                batch.Add(CreateFastEnemySpawner(prevSpawnRate, prevEnemyCount));
            }

            enemySpawners.Add(batch);

            prevSpawnRate += spawnRateIncrement;
            prevEnemyCount += enemyCountIncrement;
        }
    }

    private EnemySpawner CreateEnemySpawner(float rate, int count, Transform prefab)
    {
        return new EnemySpawner(new WaveDetails(rate, count, spawnPoint, prefab));
    }

    private EnemySpawner CreateFastEnemySpawner(float prevRate, int prevCount)
    {
        int newEnemyCount = Mathf.FloorToInt(prevCount / 2);
        float newSpawnRate = prevRate * 2f;
        return CreateEnemySpawner(newSpawnRate, newEnemyCount, fastEnemyPrefab);
    }

    private void Update()
    {
        if (gameStarted && Input.GetKeyDown(KeyCode.Space) && FindObjectsOfType<Enemy>().Length == 0)
        {
            Debug.Log("Current wave " + (currentWave + 1));
            currentSpawnerBatch = enemySpawners[currentWave];
            foreach (EnemySpawner spawner in currentSpawnerBatch)
            {
                spawner.StartGame();
            }
            currentWave++;
            infoManager.SetLevel(currentWave);
        }

        if (currentSpawnerBatch != null)
        {
            foreach (EnemySpawner spawner in currentSpawnerBatch)
            {
                spawner.Tick();
            }
        }
    }

    private void Start()
    {
        infoManager = InfoManager.instance;
    }
}
