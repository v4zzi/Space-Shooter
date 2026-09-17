using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Dificultad Progresiva")]
    public int currentLevel = 1;
    public float baseTargetDistance = 100f;       // Distancia para el Nivel 1
    public float distanceIncrementPerLevel = 50f; // Metros adicionales por nivel extra

    private float targetDistance;
    private float currentDistance;

    [Header("UI del Jugador")]
    public TMP_Text distanceText;                 // UI para mostrar metros restantes
    public TMP_Text livesText;
    public GameObject gameOverPanel;

    [Header("Ajustes del Jefe")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public Slider bossHealthSlider;
    private bool bossSpawned = false;

    [Header("Configuración de Escenas")]
    public string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Configura la distancia del nivel actual y actualiza la UI
        CalculateAndResetDistance();

        // Notifica al spawner para ajustar su velocidad en el arranque
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.IncreaseDifficulty(currentLevel);
        }
    }

    private void Update()
    {
        if (!bossSpawned)
        {
            currentDistance -= Time.deltaTime * 5f;
            currentDistance = Mathf.Max(0f, currentDistance);

            UpdateDistanceUI();

            if (currentDistance <= 0f)
            {
                SpawnBoss();
            }
        }
    }

    private void CalculateAndResetDistance()
    {
        targetDistance = baseTargetDistance + ((currentLevel - 1) * distanceIncrementPerLevel);
        currentDistance = targetDistance;
        UpdateDistanceUI();
    }

    private void UpdateDistanceUI()
    {
        if (distanceText != null)
        {
            distanceText.text = "Distance: " + Mathf.CeilToInt(currentDistance) + "m";
        }
    }

    private void SpawnBoss()
    {
        bossSpawned = true;

        if (distanceText != null)
        {
            distanceText.text = "BOSS WARNING!";
        }

        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null) spawner.enabled = false;

        if (bossPrefab != null && bossSpawnPoint != null)
        {
            GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

            BossScript bossScript = bossInstance.GetComponent<BossScript>();
            if (bossScript != null)
            {
                bossScript.maxHealth += (currentLevel - 1) * 15;
            }

            if (bossHealthSlider != null)
            {
                bossHealthSlider.gameObject.SetActive(true);
                if (bossScript != null)
                {
                    bossScript.SetupHealthBar(bossHealthSlider);
                }
            }
        }
    }

    public void OnBossDefeated()
    {
        currentLevel++;
        bossSpawned = false;

        // Recalcular la nueva distancia para el nivel recién desbloqueado
        CalculateAndResetDistance();

        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.enabled = true;
            spawner.IncreaseDifficulty(currentLevel);
        }
    }

    public void UpdateLivesUI(int currentLives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    public void TriggerGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        StartCoroutine(AutoReturnToMainMenuCoroutine(2.5f));
    }

    private IEnumerator AutoReturnToMainMenuCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToMainMenu();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}