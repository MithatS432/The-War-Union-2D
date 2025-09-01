using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float spawnDelay = 3f;
    private float spawnRate = 2f;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnDelay, spawnRate);
    }
    void SpawnEnemy()
    {
        GameObject enemyObj = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Enemy enemyScript = enemyObj.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.target = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
