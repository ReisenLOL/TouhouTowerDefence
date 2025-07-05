using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnAndMovement
    {
        public Transform spawnPoint;
        public Transform[] waypoints;
    }
    public SpawnAndMovement[] spawns;
    
    [System.Serializable]
    public class EnemySpawn
    {
        public SpawnAndMovement spawn;
        public Enemy enemyToSpawn;
        public int amountToSpawn;
    }

    [System.Serializable]
    public class Wave
    {
        public EnemySpawn[] enemies;
        public bool isBossWave;
    }

    public Wave[] waveList; //THIS IS INSANE. 3 NESTED CLASSES.

    public void SpawnWave(Wave wave)
    {
        foreach (EnemySpawn enemySpawn in wave.enemies)
        {
            for (int i = 0; i < enemySpawn.amountToSpawn; i++)
            {
                SpawnEnemy(enemySpawn);
            }
        }
    }
    public void SpawnEnemy(EnemySpawn enemyBeingSpawned)
    {
        Enemy newEnemy = Instantiate(enemyBeingSpawned.enemyToSpawn);
        newEnemy.selectedSpawnAndMovement = enemyBeingSpawned.spawn;
    }
}
