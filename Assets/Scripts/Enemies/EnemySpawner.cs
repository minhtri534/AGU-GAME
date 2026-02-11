using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;

    [Header("Wave Settings")]
    public int minEnemiesPerWave = 5;
    public int maxEnemiesPerWave = 10;
    public int totalWaves = 3;

    [Header("Spawn Area")]
    public float spawnRadius = 10f;

    private int currentWave = 0;
    private int aliveEnemies = 0;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < totalWaves)
        {
            yield return StartCoroutine(SpawnWave());

            yield return new WaitUntil(() => aliveEnemies <= 0);

            currentWave++;
        }

        Debug.Log("All waves completed!");
    }

    IEnumerator SpawnWave()
    {
        int amount = Random.Range(minEnemiesPerWave, maxEnemiesPerWave + 1);

        aliveEnemies = amount;

        for (int i = 0; i < amount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.3f);
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomXZ = transform.position +
                           new Vector3(
                               Random.Range(-spawnRadius, spawnRadius),
                               10f,
                               Random.Range(-spawnRadius, spawnRadius)
                           );

        RaycastHit hit;

        if (Physics.Raycast(randomXZ, Vector3.down, out hit, 50f))
        {
            Vector3 spawnPos = hit.point;
            spawnPos.y = 1f; 

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            EnemyController controller = enemy.GetComponent<EnemyController>();
            controller.OnEnemyDeath += HandleEnemyDeath;
        }
    }

    void HandleEnemyDeath(EnemyController enemy)
    {
        aliveEnemies--;
    }
}
