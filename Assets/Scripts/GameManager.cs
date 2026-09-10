using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Meta y Distancia")]
    public float distanceToGoal = 100f; 
    public float travelSpeed = 5f;     

    [Header("Timers de Retroalimentación")]
    public float delayBeforeMenu = 3f; 

    [Header("UI del Juego")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI distanceText;
    public GameObject winPanel;
    public GameObject losePanel;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        UpdateHighScoreUI();
    }

    private void Update()
    {
        if (isGameOver) return;

        if (distanceToGoal > 0)
        {
            distanceToGoal -= travelSpeed * Time.deltaTime;
            if (distanceText != null)
                distanceText.text = "Left: " + Mathf.Max(0, Mathf.RoundToInt(distanceToGoal)) + "m";

            if (distanceToGoal <= 0)
            {
                TriggerWin();
            }
        }
    }

    public void TriggerWin()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (winPanel != null) winPanel.SetActive(true);
        if (AudioManager.Instance != null) AudioManager.Instance.PlayWin();

        CheckHighScore();
        StartCoroutine(ReturnToMenuTimer());
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (losePanel != null) losePanel.SetActive(true);
        if (AudioManager.Instance != null) AudioManager.Instance.PlayLose();

        CheckHighScore();
        StartCoroutine(ReturnToMenuTimer());
    }

    private IEnumerator ReturnToMenuTimer()
    {
        yield return new WaitForSeconds(delayBeforeMenu);
        SceneManager.LoadScene("MainMenu");
    }

    private void CheckHighScore()
    {
        int currentScore = ScoreManager.Instance != null ? ScoreManager.Instance.currentScore : 0;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();
        }
    }

    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
            highScoreText.text = "Max: " + PlayerPrefs.GetInt("HighScore", 0);
    }

    [Header("UI de Vidas")]
    public TMPro.TextMeshProUGUI livesText; 

    public void UpdateLivesUI(int currentLives)
    {
        if (livesText != null)
        {
            livesText.text = "Vidas: " + Mathf.Max(0, currentLives);
        }
    }

}