using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Bileşenler")]
    public AudioSource audioSource;

    [Header("Müzik Listesi")]
    public AudioClip mainMenuMusic;      // 1. Ana Menü Müziği
    public AudioClip isinlanmaMusic;     // 3. Işınlanınca Çalan Müzik

    void Start()
    {
        // Sahne her yüklendiğinde (ilk açılışta veya ana menüye geri dönüldüğünde)
        // otomatik olarak menü müziğini başlatır.
        PlayMainMenuMusic();
    }

    // Müziği değiştiren genel fonksiyon
    public void PlayMusic(AudioClip clipToPlay)
    {
        if (clipToPlay == null || audioSource == null) return;
        
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

    // 2. IŞINLANINCA DİĞER ODADA ÇALAN MÜZİK
    public void PlayIsinlanmaMusic()
    {
        PlayMusic(isinlanmaMusic);
    }

    // Müziği tamamen durdurmak için
    public void StopMusic()
    {
        if (audioSource != null) audioSource.Stop();
    }
}