using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public Slider volumeSlider;

    private void Start()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void PlayGame()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayItem(); 
        SceneManager.LoadScene("SampleScene"); 
    }

    public void QuitGame()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayItem();
        Application.Quit();
        Debug.Log("Juego cerrado.");
    }

    public void SetVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume(volume);
        }
    }
}