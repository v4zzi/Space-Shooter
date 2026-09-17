using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Referencias de UI")]
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    private int currentScore = 0;
    private int highScore = 0;

    private const string HIGH_SCORE_KEY = "HighScore";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Cargar el High Score guardado en el dispositivo (0 si es la primera vez)
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

        UpdateUI();
    }

    public void AddPoints(int points)
    {
        currentScore += points;

        // Comprobar si superamos el récord actual
        if (currentScore > highScore)
        {
            highScore = currentScore;

            // Guardar inmediatamente en disco
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High: " + highScore;
        }
    }

    // Método para reiniciar el récord desde la UI (opcional)
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
        highScore = 0;
        UpdateUI();
    }
}