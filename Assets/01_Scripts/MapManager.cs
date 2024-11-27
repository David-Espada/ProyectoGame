using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Stats")]
    public int minWheatSpawn;
    public int maxWheatSpawn;
    [Header("References")]
    public List<Transform> SpawnPoints;
    public GameObject wheatPrefab;
    public GameObject workerSheep;
    public float workerPrice;
    public Transform sheepSpawnpoint;
    public Player player;
    [Header("Internal")]
    public List<GameObject> Wheats;
    public List<Sheep> sheeps;

    void Start()
    {
        GenerateWheats();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void GenerateWheats()
    {
        // Genera el n?mero m?ximo de spawns
        maxWheatSpawn = Random.Range(minWheatSpawn, SpawnPoints.Count);

        // Crea una copia de la lista de SpawnPoints para evitar repetir
        List<Transform> availableSpawns = new List<Transform>(SpawnPoints);
        RemoveWheats();
        for (int i = 0; i < maxWheatSpawn; i++)
        {
            // Selecciona un ?ndice aleatorio de la lista de disponibles
            int randomIndex = Random.Range(0, availableSpawns.Count);

            // Obtiene el punto de spawn aleatorio
            Transform randSpawn = availableSpawns[randomIndex];

            // Instancia el prefab en el punto aleatorio
            GameObject spawnedWheat = Instantiate(wheatPrefab, randSpawn.position, Quaternion.identity);

            // A?ade el objeto a la lista de Wheats
            Wheats.Add(spawnedWheat);

            // Elimina el punto usado de la lista de disponibles
            availableSpawns.RemoveAt(randomIndex);
        }
    }

    public void RemoveWheats()
    {
        Wheats.ForEach(w =>
        {
            Destroy(w);
        });
        Wheats.Clear();
    }
    public void SpawnWorkerSheep()
    {
        if (player.money >= workerPrice )
        {
            player.SpendMoney(workerPrice);
            Sheep newSheep = Instantiate(workerSheep, sheepSpawnpoint.position, Quaternion.identity).GetComponent<Sheep>();
            sheeps.Add(newSheep);
            newSheep.WheatPlaces = Wheats;
        }
        
    }
   
}
