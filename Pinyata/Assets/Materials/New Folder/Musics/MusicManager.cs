using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Bileşenler")]
    public AudioSource audioSource;

    [Header("Müzik Listesi")]
    public AudioClip mainMenuMusic;      // 1. Ana Menü Müziği
    public AudioClip genelOyunMusic;     // 2. Oyun Başlayınca Çalan Genel Müzik
    public AudioClip isinlanmaMusic;     // 3. Işınlanınca Çalan Müzik

    void Awake()
    {
        // Sahnede sadece BİR tane MusicManager olduğundan emin oluyoruz
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değiştiğinde yok olmasını engeller
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Menüde otomatik olarak menü müziğini başlat
        if (mainMenuMusic != null)
        {
            PlayMusic(mainMenuMusic);
        }
    }

    // Müziği değiştiren genel fonksiyon
    public void PlayMusic(AudioClip clipToPlay)
    {
        if (clipToPlay == null) return;
        
        // Eğer zaten o müzik çalıyorsa baştan başlatma
        if (audioSource.clip == clipToPlay && audioSource.isPlaying)
            return;

        audioSource.Stop();
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }

    // 1. ANA MENÜ MÜZİĞİNİ ÇAL
    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic);
    }

    // 2. GENEL OYUN MÜZİĞİNİ ÇAL
    public void PlayGenelOyunMusic()
    {
        PlayMusic(genelOyunMusic);
    }

    // 3. IŞINLANINCA DİĞER ODADA ÇALAN MÜZİK
    public void PlayIsinlanmaMusic()
    {
        PlayMusic(isinlanmaMusic);
    }

    // Gerekirse müziği tamamen durdurmak için
    public void StopMusic()
    {
        audioSource.Stop();
    }
}