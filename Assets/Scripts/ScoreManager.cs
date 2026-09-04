using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    
    public static ScoreManager Instance;

    [Header("Puntuación")]
    public int currentScore = 0;

    [Header("UI")]
    public TextMeshProUGUI scoreText; 

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    
    public void AddPoints(int points)
    {
        currentScore += points;
        UpdateScoreUI();
    }

    
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }
}