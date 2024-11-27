using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public int spawnRate;
    public List<GameObject> Enemys;
    public List<Transform> EnemySpawnPoints;

    public void GenerateEnemys()
    {
        for (int i = 0; i < spawnRate; i++)
        {
            Vector3 randPosition = EnemySpawnPoints[Random.Range(0, EnemySpawnPoints.Count)].position;
            GameObject newEnemy = Enemys[Random.Range(0, Enemys.Count)];
            Instantiate(newEnemy,randPosition,Quaternion.identity);
        }
    }

}
