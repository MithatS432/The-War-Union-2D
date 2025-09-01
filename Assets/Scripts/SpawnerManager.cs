using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawners;
    public float spawnDelay = 5f;
    public float spawnRate = 8f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnWave), spawnDelay, spawnRate);
    }

    void SpawnWave()
    {
        int activeSpawnerCount = Random.Range(1, spawners.Length + 1);

        Transform[] shuffled = (Transform[])spawners.Clone();
        for (int i = 0; i < shuffled.Length; i++)
        {
            int rand = Random.Range(i, shuffled.Length);
            (shuffled[i], shuffled[rand]) = (shuffled[rand], shuffled[i]);
        }

        for (int i = 0; i < activeSpawnerCount; i++)
        {
            int enemiesToSpawn = Random.Range(1, 4);

            for (int j = 0; j < enemiesToSpawn; j++)
            {
                GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                GameObject enemyObj = Instantiate(prefabToSpawn, shuffled[i].position, Quaternion.identity);
                Enemy enemyScript = enemyObj.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.target = GameObject.FindGameObjectWithTag("Player");
                }
            }
        }
    }
}
