using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnRate;

    private float lastSpawn;


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void SpawnEnemies()
    {
        if (Time.deltaTime < spawnRate)
        {
            lastSpawn = Time.deltaTime;
            
            GameObject newEnemy = Instantiate(enemy);
        }
    }
}
