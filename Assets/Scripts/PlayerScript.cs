using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public GameObject PlayerBullet;
    public Transform firePoint;

    public InputAction controller;     
    public InputAction shootAction;     

    public Rigidbody rb;
    public float speed = 10f;

    [Header("Límites de Movimiento (Ejes X y Z)")]
    public float xMin = -8f;
    public float xMax = 8f;
    public float zMin = -5f;
    public float zMax = 5f;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        controller.Enable();
        shootAction.Enable();

        
        shootAction.performed += OnShootPerformed;
    }

    private void OnDisable()
    {
        controller.Disable();
        shootAction.Disable();

        shootAction.performed -= OnShootPerformed;
    }

    private void FixedUpdate()
    {
        Vector2 move = controller.ReadValue<Vector2>();

        Vector3 direction = new Vector3(move.x, 0f, move.y);

        Vector3 targetPosition = transform.position + (direction * speed * Time.fixedDeltaTime);

        targetPosition.x = Mathf.Clamp(targetPosition.x, xMin, xMax);
        targetPosition.z = Mathf.Clamp(targetPosition.z, zMin, zMax);

        rb.MovePosition(targetPosition);
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        Shoot();
    }

    void Shoot()
    {
        if (PlayerBullet != null && firePoint != null)
        {
            Instantiate(PlayerBullet, firePoint.position, firePoint.rotation);
        }
    }
    [Header("Salud / Vidas")]
    public int lives = 3;

    public void TakeDamage(int damageAmount)
    {
        lives -= damageAmount;
        Debug.Log("Vida restante: " + lives);

        if (lives <= 0)
        {
            
            gameObject.SetActive(false);
        }
    }

    public void AddLife(int extraLives)
    {
        lives += extraLives;
        Debug.Log("¡Vida extra! Total vidas: " + lives);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1); 

            EnemyScript enemy = other.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.ResetEnemyPosition(); 
            }
        }
    }

}