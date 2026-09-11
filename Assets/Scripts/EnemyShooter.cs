using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public float speed = 3f;
    public int pointsValue = 150;

    [Header("Disparo")]
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float fireTimer;

    private void Update()
    {
        // Movimiento constante hacia abajo
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        // Control de disparos
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }

        if (transform.position.z < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void Shoot()
    {
        if (enemyBulletPrefab != null && firePoint != null)
        {
            Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.Euler(0, 180, 0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            ScoreManager.Instance?.AddPoints(pointsValue);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();
            player?.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}