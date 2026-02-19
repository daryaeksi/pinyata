using System.Collections;
using UnityEngine;

public class IntroStart : MonoBehaviour
{
    [Header("Kameralar")]
    public GameObject cutsceneCamera;     // Sinematik Kamera
    public GameObject mainCamera;         // Oyuncunun Takip Kamerası

    [Header("Objeler")]
    public GameObject animCharacterIntro; // Ara sahnedeki hareketli karakter
    public GameObject playerCharacter;    // Gerçek oyuncu kontrolcüsü

    [Header("UI & Animasyon")]
    public GameObject uiBlackImage;       // Kararma efektini yapan Siyah Resim
    private Animator uiAnimator;

    [Header("Zaman Ayarları")]
    public float introTime = 5f;          // Ara sahne kaç saniye sonra bitsin?
    public float transitionTime = 2f;     // Kararma hızı (Fade süresi)

    void Start()
    {
        // UI nesnesindeki Animator'ı bul
        if (uiBlackImage != null)
            uiAnimator = uiBlackImage.GetComponent<Animator>();

        // OYUN BAŞI AYARLARI:
        // Sinematik açık, oyuncu ve kamerası kapalı başlar.
        playerCharacter.SetActive(false);
        mainCamera.SetActive(false);
        animCharacterIntro.SetActive(true);
        cutsceneCamera.SetActive(true);

        // Süreci başlat
        StartCoroutine(IntroCutRoutine());
    }

    IEnumerator IntroCutRoutine()
    {
        // 1. Ara sahnenin (Timeline) bitmesini bekle
        yield return new WaitForSeconds(introTime);

        // 2. Ekranı karartmaya başla (Trigger'ı ateşle)
        if (uiAnimator != null)
            uiAnimator.SetTrigger("Play");

        // 3. Ekran tam kapandığında (transitionTime'ın ortası) geçişi yap
        yield return new WaitForSeconds(transitionTime / 2);

        // Sinematiği kapat
        animCharacterIntro.SetActive(false);
        cutsceneCamera.SetActive(false);

        // Oyuncuyu ve kamerasını aç
        playerCharacter.SetActive(true);
        mainCamera.SetActive(true);

        Debug.Log("Geçiş Tamamlandı: Oyuncu kontrolü devraldı.");
    }
}