using UnityEngine;
using TMPro; // Yazı kutusu (TextMeshPro) için bu şart
using System.Collections;

public class AltyaziSistemi : MonoBehaviour
{
    [Header("Ayarlar")]
    public TextMeshProUGUI yaziKutusu; // Sahnedeki TextMeshPro objesini buraya sürükle
    public float harfHizi = 0.04f;    // Harflerin çıkış hızı (Daha küçük = Daha hızlı)
    public float beklemeSuresi = 2.0f; // Yazı bittikten sonra ekranda kalma süresi

    // Timeline'dan çağıracağımız fonksiyon bu
    public void AltyaziYaz(string mesaj)
    {
        // Eğer o sırada zaten bir yazı yazılıyorsa onu durdur (yazılar çakışmasın)
        StopAllCoroutines();
        // Yeni yazıyı daktilo efektiyle başlat
        StartCoroutine(DaktiloYazdir(mesaj));
    }

    IEnumerator DaktiloYazdir(string mesaj)
    {
        yaziKutusu.text = ""; // Önce ekranı bir temizleyelim

        // Mesajdaki her bir harfi tek tek ekrana basar
        foreach (char harf in mesaj.ToCharArray())
        {
            yaziKutusu.text += harf; 
            // Her harf arası minik bir bekleme (daktilo hissi)
            yield return new WaitForSeconds(harfHizi);
        }

        // Yazı bitti, şimdi oyuncu okusun diye biraz daha bekleyelim
        yield return new WaitForSeconds(beklemeSuresi);

        // Ve ekranı temizle
        yaziKutusu.text = "";
    }
}