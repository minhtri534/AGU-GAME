using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;

    [Header("Wave Settings")]
    public WaveData data;

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
        while (currentWave < data.Waves.Count)
        {
            yield return StartCoroutine(SpawnWave());

            yield return new WaitUntil(() => aliveEnemies <= 0);

            currentWave++;
        }

        Debug.Log("All waves completed!");
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < data.Waves[currentWave].FatRat; i++)
        {
            SpawnEnemy(EnemyType.FatRat);
            //yield return new WaitForSeconds(0.3f);
            aliveEnemies++;
        }
        for (int i = 0; i < data.Waves[currentWave].TallRat; i++)
        {
            SpawnEnemy(EnemyType.TallRat);
            //yield return new WaitForSeconds(0.3f);
            aliveEnemies++;
        }
        for (int i = 0; i < data.Waves[currentWave].ShortRat; i++)
        {
            SpawnEnemy(EnemyType.ShortRat);
            
            aliveEnemies++;
        }
        yield return new WaitForSeconds(0.3f);
    }

    void SpawnEnemy(EnemyType type)
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

            GameObject enemy = PhotonNetwork.Instantiate("Prefabs/Enemies/Enemy", spawnPos, Quaternion.identity);

            EnemyController controller = enemy.GetComponent<EnemyController>();

            // Set enemy data

            var gun = enemy.AddComponent<Gun>();

            
            gun.isEnemyWeapon = true;
            gun.firePoint = enemy.transform;

            // Per enemy data
            switch (type)
            {
                case EnemyType.TallRat:
                    gun.inventory.SwapBehaviourModifierComponent(1, new TallRatBehaviourComponent());
                    controller.StateMachine = new TallRatStateMachine();
                    enemy.GetComponentInChildren<MeshRenderer>().material.mainTexture = Resources.Load<Texture>("Sprites/tall_rat_idle");
                    break;
                case EnemyType.FatRat:
                    gun.inventory.SwapBehaviourModifierComponent(1, new FatRatBehaviourComponent());
                    controller.StateMachine = new FatRatStateMachine();
                    enemy.GetComponentInChildren<MeshRenderer>().material.mainTexture = Resources.Load<Texture>("Sprites/fat_rat_idle");
                    break;
                case EnemyType.ShortRat:
                    gun.inventory.SwapBehaviourModifierComponent(1, new ShortRatBehaviourComponent());
                    controller.StateMachine = new ShortRatStateMachine();
                    enemy.GetComponentInChildren<MeshRenderer>().material.mainTexture = Resources.Load<Texture>("Sprites/short_rat_idle");
                    break;
            }


            controller.OnEnemyDeath += HandleEnemyDeath;
        }
    }

    void HandleEnemyDeath(EnemyController enemy)
    {
        aliveEnemies--;
    }
}

public enum EnemyType
{
    TallRat,
    FatRat,
    ShortRat,
}
