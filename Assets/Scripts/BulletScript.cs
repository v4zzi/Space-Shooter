using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Parámetros de la Bala")]
    public float speed = 20f;
    public int damage = 1;
    public float lifetime = 3f; 

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); 
            }

            Destroy(gameObject);
        }
    }
}