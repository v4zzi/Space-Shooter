using UnityEngine;

public class BossBulletScript : MonoBehaviour
{
    public float speed = 8f;      // Velocidad hacia abajo
    public int damage = 1;        // Daño al jugador
    public float lifetime = 5f;   // Autodestrucción por tiempo

    private void Start()
    {
        // Se destruye automáticamente después de 'lifetime' segundos si no choca
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Avanza hacia abajo en el eje Z (hacia el jugador)
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si choca con el Jugador
        if (other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.TakeDamage(damage); // Aplica el daño y descuenta vida
            }

            Destroy(gameObject); // Destruye la bala al impactar
        }
    }
}