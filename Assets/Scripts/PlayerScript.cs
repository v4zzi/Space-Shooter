using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Header("Referencias Base")]
    public GameObject PlayerBullet;
    public Transform firePoint;
    public Rigidbody rb;

    [Header("Input Actions")]
    public InputAction controller;
    public InputAction shootAction;

    [Header("Movimiento")]
    public float speed = 10f;
    public float xMin = -8f;
    public float xMax = 8f;
    public float zMin = -5f;
    public float zMax = 5f;

    [Header("Sistema de Disparo y Cadencia")]
    public float baseFireRate = 0.25f;
    private float currentFireRate;
    private float nextFireTime = 0f;
    private bool isShootingPressed = false;

    [Header("Power-Ups")]
    public bool isTripleShotActive = false;
    public bool isRapidFireActive = false;
    public float powerUpDuration = 6f;

    private float tripleShotTimer = 0f;
    private float rapidFireTimer = 0f;

    [Header("Salud / Vidas")]
    public int lives = 3;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        currentFireRate = baseFireRate;
        UpdateLivesInUI();
    }

    private void OnEnable()
    {
        controller.Enable();
        shootAction.Enable();

        shootAction.started += OnShootStarted;
        shootAction.canceled += OnShootCanceled;
    }

    private void OnDisable()
    {
        controller.Disable();
        shootAction.Disable();

        shootAction.started -= OnShootStarted;
        shootAction.canceled -= OnShootCanceled;
    }

    private void Update()
    {
        HandlePowerUpTimers();

        if (isShootingPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + currentFireRate;
        }
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

    private void OnShootStarted(InputAction.CallbackContext context) => isShootingPressed = true;
    private void OnShootCanceled(InputAction.CallbackContext context) => isShootingPressed = false;

    private void Shoot()
    {
        if (PlayerBullet == null || firePoint == null) return;

        if (isTripleShotActive)
        {
            Instantiate(PlayerBullet, firePoint.position, firePoint.rotation);

            Quaternion leftRotation = Quaternion.Euler(0, firePoint.rotation.eulerAngles.y - 15f, 0);
            Quaternion rightRotation = Quaternion.Euler(0, firePoint.rotation.eulerAngles.y + 15f, 0);

            Instantiate(PlayerBullet, firePoint.position, leftRotation);
            Instantiate(PlayerBullet, firePoint.position, rightRotation);
        }
        else
        {
            Instantiate(PlayerBullet, firePoint.position, firePoint.rotation);
        }

        AudioManager.Instance?.PlayShoot();
    }

    public void ActivateRapidFire()
    {
        isRapidFireActive = true;
        rapidFireTimer = powerUpDuration;
        currentFireRate = baseFireRate * 0.4f;
    }

    public void ActivateTripleShot()
    {
        isTripleShotActive = true;
        tripleShotTimer = powerUpDuration;
    }

    private void HandlePowerUpTimers()
    {
        if (isRapidFireActive)
        {
            rapidFireTimer -= Time.deltaTime;
            if (rapidFireTimer <= 0)
            {
                isRapidFireActive = false;
                currentFireRate = baseFireRate;
            }
        }

        if (isTripleShotActive)
        {
            tripleShotTimer -= Time.deltaTime;
            if (tripleShotTimer <= 0)
            {
                isTripleShotActive = false;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        lives -= damageAmount;
        UpdateLivesInUI();

        if (lives <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }

            gameObject.SetActive(false);
        }
    }

    public void AddLife(int extraLives)
    {
        lives += extraLives;
        UpdateLivesInUI();
    }

    private void UpdateLivesInUI()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateLivesUI(lives);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Las colisiones directas se gestionan desde los scripts de los enemigos/ítems
    }
}