using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Geçiş Elemanları")]
    public Image karanlikPerde; // Ekranı kaplayan siyah Image objesi
    public float kararmaHizi = 1.5f;

    [Header("Kapatılacak Menü Objeleri")]
    public GameObject menuKamerasi;
    public GameObject ekranKamerasi;
    public GameObject mainMenuCanvas;

    [Header("Açılacak Oyun Objeleri")]
    public GameObject asilOyunKamerasi; // Oyuna girdiğinde aktif olacak kamera veya karakter

    public void NewGameBasildi()
    {
        // Butona tıklandığında kararma ve geçiş sürecini başlatır
        StartCoroutine(OyunuBaslatRutini());
    }

    IEnumerator OyunuBaslatRutini()
    {
        // Siyah perdeyi aktif et ve yavaşça görünür (Alpha = 1) yap
        if (karanlikPerde != null)
        {
            karanlikPerde.gameObject.SetActive(true);
            Color renk = karanlikPerde.color;
            renk.a = 0f;
            karanlikPerde.color = renk;

            while (renk.a < 1f)
            {
                renk.a += Time.deltaTime * kararmaHizi;
                karanlikPerde.color = renk;
                yield return null;
            }
        }

        // Ekran tamamen simsiyah olduğunda menü elemanlarını kapatıyoruz
        if (menuKamerasi != null) menuKamerasi.SetActive(false);
        if (ekranKamerasi != null) ekranKamerasi.SetActive(false);
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(false);

        // Asıl oyunun kamerasını/karakterini açıyoruz
        if (asilOyunKamerasi != null)
        {
            asilOyunKamerasi.SetActive(true);
        }

        // İsteğe bağlı: Oyun başlayınca siyah perde tekrar yavaşça açılsın (şeffaf olsun)
        if (karanlikPerde != null)
        {
            Color renk = karanlikPerde.color;
            while (renk.a > 0f)
            {
                renk.a -= Time.deltaTime * kararmaHizi;
                karanlikPerde.color = renk;
                yield return null;
            }
            karanlikPerde.gameObject.SetActive(false);
        }
    }
}