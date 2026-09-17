using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Variedad de Enemigos")]
    public GameObject[] enemyPrefabs;

    [Header("Cadencia de Aparición")]
    public float initialSpawnRate = 2f;
    public float minSpawnRate = 0.5f;
    public float rateReductionPerLevel = 0.3f; // Reducción de tiempo por cada nivel
    private float currentSpawnRate;
    private float timer;

    [Header("Límites de Generación")]
    public float xMin = -7f;
    public float xMax = 7f;
    public float zSpawn = 15f;

    private void Start()
    {
        // Sincroniza la velocidad de aparición con el nivel actual registrado en GameManager
        if (GameManager.Instance != null)
        {
            IncreaseDifficulty(GameManager.Instance.currentLevel);
        }
        else
        {
            currentSpawnRate = initialSpawnRate;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SpawnRandomEnemy();
            timer = 0f;
        }
    }

    private void SpawnRandomEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        float randomX = Random.Range(xMin, xMax);

        Vector3 spawnPos = new Vector3(randomX, transform.position.y, zSpawn);
        Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);
    }

    // Calcula y aplica la nueva cadencia de aparición basada en el nivel
    public void IncreaseDifficulty(int level)
    {
        // Restamos (level - 1) para que en el Nivel 1 se respete el initialSpawnRate completo
        float targetRate = initialSpawnRate - ((level - 1) * rateReductionPerLevel);

        // Aplica la cadencia garantizando que no baje del límite mínimo
        currentSpawnRate = Mathf.Max(minSpawnRate, targetRate);
    }
}