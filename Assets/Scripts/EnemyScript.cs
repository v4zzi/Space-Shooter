using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Velocidad y Patrón")]
    public float speedForward = 4f;
    public float frequency = 2.5f;
    public float magnitude = 3f;

    [Header("Salud y Puntos")]
    public int health = 1;
    public int scoreValue = 100;

    [Header("Drop de Ítems (Power-ups)")]
    public GameObject dropItemPrefab; 
    [Range(0f, 1f)]
    public float dropChance = 0.3f;   

    [Header("Límites de Reaparición")]
    public float zLimitBottom = -10f;
    public float zSpawnTop = 15f;
    public float xSpawnMin = -7f;
    public float xSpawnMax = 7f;

    private Vector3 startPosition;
    private float timer;
    private int currentHealth;

    void Start()
    {
        ResetEnemyPosition();
    }

    void Update()
    {
        timer += Time.deltaTime;

        float newZ = transform.position.z - (speedForward * Time.deltaTime);
        float newX = startPosition.x + Mathf.Sin(timer * frequency) * magnitude;

        transform.position = new Vector3(newX, transform.position.y, newZ);

        if (transform.position.z <= zLimitBottom)
        {
            ResetEnemyPosition();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(scoreValue);
        }

        
        TryDropItem();

        
        ResetEnemyPosition();
    }

    private void TryDropItem()
    {
        if (dropItemPrefab != null)
        {
            float randomValue = Random.value; 
            if (randomValue <= dropChance)
            {
                Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    public void ResetEnemyPosition()
    {
        currentHealth = health; 
        float randomX = Random.Range(xSpawnMin, xSpawnMax);

        startPosition = new Vector3(randomX, transform.position.y, zSpawnTop);
        transform.position = startPosition;

        timer = 0f;
    }
}