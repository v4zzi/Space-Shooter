using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public enum ItemType { ExtraPoints, ExtraLife, TripleShot, RapidFire }

    [Header("Configuración del Ítem")]
    public ItemType type;
    public int value = 1;
    public float fallSpeed = 2f;

    private void Update()
    {
        transform.Translate(Vector3.back * fallSpeed * Time.deltaTime, Space.World);

        if (transform.position.z < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();

            if (player != null)
            {
                switch (type)
                {
                    case ItemType.RapidFire:
                        player.ActivateRapidFire();
                        break;
                    case ItemType.TripleShot:
                        player.ActivateTripleShot();
                        break;
                    case ItemType.ExtraLife:
                        player.AddLife(value);
                        break;
                    case ItemType.ExtraPoints:
                        ScoreManager.Instance?.AddPoints(value * 100);
                        break;
                }

                AudioManager.Instance?.PlayItem();
            }

            Destroy(gameObject);
        }
    }
}