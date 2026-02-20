using UnityEngine;

public class RadioController : MonoBehaviour
{
    [Header("Ses Ayarlari")]
    public AudioSource musicSource;
    public AudioSource clickSource;
    public AudioClip clickSound;

    [Header("Gorsel Ayarlar")]
    public GameObject radioLight; // Buraya Point Light'ı sürükle

    public bool isOn = false;

    void Start()
    {
        if (musicSource == null) musicSource = GetComponent<AudioSource>();

        // Başlangıç ayarları
        if (musicSource != null)
        {
            musicSource.playOnAwake = true;
            musicSource.loop = true;
            musicSource.mute = true; 
            musicSource.Play();
        }

        // Işık başlangıçta kapalı olsun
        if (radioLight != null) radioLight.SetActive(false);
    }

    public void Interact()
    {
        // 1. Tık sesini çal
        if (clickSource != null && clickSound != null)
        {
            clickSource.PlayOneShot(clickSound);
        }

        // 2. Durumu değiştir
        isOn = !isOn;

        // 3. Müziği aç/kapat
        if (musicSource != null)
        {
            musicSource.mute = !isOn;
        }

        // 4. Işığı aç/kapat
        if (radioLight != null)
        {
            radioLight.SetActive(isOn);
        }

        Debug.Log(isOn ? "Radyo ve Işık Açıldı" : "Radyo ve Işık Kapatıldı");
    }
}