using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnAndMovement
    {
        public Transform spawnPoint;
        public List<Transform> waypoints = new();
    }
    [System.Serializable]
    public class EnemyGroup
    {
        public SpawnAndMovement spawn;
        public Enemy enemyToSpawn;
        public int amountToSpawn;
    }

    [System.Serializable]
    public class Wave
    {
        public EnemyGroup[] enemies;
        public bool isBossWave;
    }
    public Wave[] waveList; //THIS IS INSANE. 3 NESTED CLASSES.
    public int currentWave;
    public float timeBetweenEnemySpawns;
    private GameManager gameManager;
    public Transform enemiesFolder;
    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        foreach (Wave wave in waveList)
        {
            foreach (EnemyGroup enemyGroup in wave.enemies)
            {
                enemyGroup.spawn.waypoints = enemyGroup.spawn.spawnPoint.GetComponentsInChildren<Transform>().ToList();
                enemyGroup.spawn.waypoints.RemoveAt(0);
                gameManager.totalEnemyCount += enemyGroup.amountToSpawn;
            }
        }
        gameManager.UpdateEnemyCountUI();
        StartCoroutine(SpawnWave(waveList[currentWave]));
    }

    private void Update()
    {
        
    }

    public void DebugSpawnNextWave()
    {
        currentWave++;
        StartCoroutine(SpawnWave(waveList[currentWave]));
    }
    IEnumerator SpawnWave(Wave wave)
    {
        int currentEnemyGroup = 0;
        while (currentEnemyGroup < wave.enemies.Length)
        {
            int currentEnemyNumber = 0;
            while (currentEnemyNumber < wave.enemies[currentEnemyGroup].amountToSpawn)
            {
                Enemy newEnemy = Instantiate(wave.enemies[currentEnemyGroup].enemyToSpawn, enemiesFolder);
                newEnemy.selectedSpawnAndMovement = wave.enemies[currentEnemyGroup].spawn;
                newEnemy.transform.position = wave.enemies[currentEnemyGroup].spawn.spawnPoint.position;
                currentEnemyNumber++;
                yield return new WaitForSeconds(timeBetweenEnemySpawns);
            }
            currentEnemyGroup++;
            yield return new WaitForSeconds(timeBetweenEnemySpawns);
        }

        if (currentWave != waveList.Length)
        {
            currentWave++;
            StartCoroutine(SpawnWave(waveList[currentWave]));
        }
        yield return null;
    }   
}
