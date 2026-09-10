using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGM();
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

    public void PlayShoot() => sfxShootSource?.PlayOneShot(shootClip);
    public void PlayItem() => sfxItemSource?.PlayOneShot(itemClip);
    public void PlayWin() => sfxGameEndSource?.PlayOneShot(winClip);
    public void PlayLose() => sfxGameEndSource?.PlayOneShot(loseClip);

    
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}