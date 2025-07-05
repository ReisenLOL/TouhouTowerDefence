using System;
using System.Collections.Generic;
using System.Linq;
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
    private void Start()
    {
        foreach (Wave wave in waveList)
        {
            foreach (EnemyGroup enemyGroup in wave.enemies)
            {
                enemyGroup.spawn.waypoints = enemyGroup.spawn.spawnPoint.GetComponentsInChildren<Transform>().ToList();
                enemyGroup.spawn.waypoints.RemoveAt(0);
            }
        }
        SpawnWave(waveList[0]);
    }

    private void Update()
    {
        
    }

    public void DebugSpawnNextWave()
    {
        currentWave++;
        SpawnWave(waveList[currentWave]);
    }
    public void SpawnWave(Wave wave)
    {
        foreach (EnemyGroup enemySpawn in wave.enemies)
        {
            for (int i = 0; i < enemySpawn.amountToSpawn; i++)
            {
                SpawnEnemy(enemySpawn);
            }
        }
    }   
    public void SpawnEnemy(EnemyGroup enemyBeingSpawned)
    {
        Enemy newEnemy = Instantiate(enemyBeingSpawned.enemyToSpawn);
        newEnemy.selectedSpawnAndMovement = enemyBeingSpawned.spawn;
        newEnemy.transform.position = enemyBeingSpawned.spawn.spawnPoint.position;
    }
}
