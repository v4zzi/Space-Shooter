using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxShootSource;
    public AudioSource sfxItemSource;
    public AudioSource sfxGameEndSource;

    [Header("Clips de Sonido")]
    public AudioClip bgmClip;
    public AudioClip shootClip;
    public AudioClip itemClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    private void Awake()
    {
        // Control de Singleton estricto
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        // Escuchar cuando cambie o se recargue la escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayBGM();
    }

    // Se ejecuta autom�ticamente CADA VEZ que se carga o reinicia una escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-asegurar que el AudioListener global est� habilitado
        AudioListener.volume = 1f;

        // Asegurar que la m�sica de fondo siga o vuelva a sonar si se detuvo
        if (bgmSource != null && bgmClip != null)
        {
            if (!bgmSource.isPlaying)
            {
                PlayBGM();
            }
        }
    }

    public void PlayBGM()
    {
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlayShoot()
    {
        if (sfxShootSource != null && shootClip != null)
            sfxShootSource.PlayOneShot(shootClip);
    }

    public void PlayItem()
    {
        if (sfxItemSource != null && itemClip != null)
            sfxItemSource.PlayOneShot(itemClip);
    }

    public void PlayWin()
    {
        if (sfxGameEndSource != null && winClip != null)
            sfxGameEndSource.PlayOneShot(winClip);
    }

    public void PlayLose()
    {
        if (sfxGameEndSource != null && loseClip != null)
            sfxGameEndSource.PlayOneShot(loseClip);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = Mathf.Clamp01(volume);
    }
}