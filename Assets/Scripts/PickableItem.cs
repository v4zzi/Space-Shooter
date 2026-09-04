using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public enum ItemType { ExtraPoints, ExtraLife }

    [Header("Tipo y Valor")]
    public ItemType type = ItemType.ExtraPoints;
    public int value = 250;       // Cantidad de puntos o vidas a sumar

    [Header("Movimiento")]
    public float fallSpeed = 2f;  // Velocidad a la que cae hacia el jugador

    void Update()
    {
        // Avanza hacia abajo en el eje Z
        transform.Translate(Vector3.back * fallSpeed * Time.deltaTime, Space.World);

        // Se autodestruye si pasa al jugador y sale de pantalla
        if (transform.position.z < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (type == ItemType.ExtraPoints)
            {
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.AddPoints(value);
                }
            }
            else if (type == ItemType.ExtraLife)
            {
                PlayerScript player = other.GetComponent<PlayerScript>();
                if (player != null)
                {
                    player.AddLife(value);
                }
            }

            
            Destroy(gameObject);
        }
    }
}