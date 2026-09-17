using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    [Header("Salud del Jefe")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Barra de Vida (UI)")]
    private Slider healthSlider;

    [Header("Movimiento del Jefe")]
    public float speed = 4f;
    public float movementRange = 6f;
    private Vector3 startPosition;

    [Header("Sistema de Disparo del Jefe")]
    public GameObject enemyBulletPrefab; // Prefab de la bala del enemigo
    public Transform firePoint;          // Lugar de donde salen los disparos
    public float fireRate = 1.5f;        // Cadencia de disparo (segundos)
    private float fireTimer;

    [Header("Puntos de Recompensa")]
    public int scoreValue = 500;

    [Header("Sistema de Drops / Recompensas")]
    public GameObject[] possibleDrops;
    [Range(0f, 1f)] public float dropChance = 1.0f;

    private void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void Update()
    {
        MoveBoss();
        HandleShooting();
    }

    private void MoveBoss()
    {
        // Movimiento senoidal liso en X (asegura que siempre se mueva)
        float newX = startPosition.x + Mathf.Sin(Time.time * speed) * movementRange;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void HandleShooting()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    private void Shoot()
    {
        if (enemyBulletPrefab != null)
        {
            // Usar el firePoint si está asignado; de lo contrario, disparar desde la posición del jefe
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

            // Instanciar el proyectil con rotación apuntando hacia abajo (180° en Y)
            Instantiate(enemyBulletPrefab, spawnPos, Quaternion.Euler(0f, 180f, 0f));
        }
    }

    public void SetupHealthBar(Slider slider)
    {
        healthSlider = slider;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ScoreManager.Instance?.AddPoints(scoreValue);

        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false);
        }

        DropItem();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBossDefeated();
        }

        AudioManager.Instance?.PlayWin();
        Destroy(gameObject);
    }

    private void DropItem()
    {
        if (possibleDrops != null && possibleDrops.Length > 0 && Random.value <= dropChance)
        {
            int randomIndex = Random.Range(0, possibleDrops.Length);
            GameObject selectedDrop = possibleDrops[randomIndex];

            if (selectedDrop != null)
            {
                Instantiate(selectedDrop, transform.position, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(10);
            Destroy(other.gameObject);
        }
    }
}