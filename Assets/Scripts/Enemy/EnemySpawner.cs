using System.Collections; // Required for Coroutines
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Setup")]
    public GameObject enemyPrefab;
    public Transform playerTransform;

    [Header("Spawning Settings")]
    public float spawnInterval = 5f;
    public float spawnRadius = 10f;

    private void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (playerTransform != null && enemyPrefab != null)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        Vector3 spawnOffset = new Vector3(randomDirection.x, 0, randomDirection.y) * spawnRadius;

        Vector3 spawnPosition = playerTransform.position + spawnOffset;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        Debug.Log("Spawned a new enemy at " + spawnPosition);
    }
}
