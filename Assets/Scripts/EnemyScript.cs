using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Estadísticas Base del Enemigo")]
    public int maxHealth = 1;
    protected int currentHealth;
    public int pointsValue = 100;
    public float speed = 4f;

    [Header("Sistema de Drops")]
    [Range(0f, 1f)] public float dropChance = 0.3f;
    public GameObject[] itemPrefabs;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        Move();

        if (transform.position.z < -12f)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Move()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    public virtual void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(pointsValue);
        }

        TrySpawnDrop();
        Destroy(gameObject);
    }

    protected void TrySpawnDrop()
    {
        if (itemPrefabs != null && itemPrefabs.Length > 0)
        {
            float randomValue = Random.value;
            if (randomValue <= dropChance)
            {
                int randomIndex = Random.Range(0, itemPrefabs.Length);
                Instantiate(itemPrefabs[randomIndex], transform.position, Quaternion.identity);
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }
}