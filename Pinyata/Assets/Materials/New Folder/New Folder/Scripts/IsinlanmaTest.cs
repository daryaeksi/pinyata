using UnityEngine;
using System.Collections;

public class IsinlanmaTesti : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform oyuncu;
    public Transform oda1Merkez; 
    public Transform oda2Merkez; 

    [Header("Atmosfer Ayarları")]
    public Material kirmiziGokyuzu;
    public Material normalGokyuzu; // Normal gökyüzünü buraya koyacağız
    public Color sisRengi = new Color(0.5f, 0, 0);
    public float sisYogunlugu = 0.05f;

    private bool oda2deyim = false; // Hangi odada olduğumuzu takip eder
    private bool isinlanmaBasladi = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isinlanmaBasladi)
        {
            StartCoroutine(GlitchIsinlanma());
        }
    }

    IEnumerator GlitchIsinlanma()
    {
        isinlanmaBasladi = true;

        // Gidilecek yönü belirle
        Vector3 odaFarki = oda2deyim ? (oda1Merkez.position - oda2Merkez.position) : (oda2Merkez.position - oda1Merkez.position);

        // --- GLITCH EFEKTİ ---
        for (int i = 0; i < 3; i++) 
        {
            oyuncu.position += odaFarki;
            yield return new WaitForSeconds(0.05f);
            oyuncu.position -= odaFarki;
            yield return new WaitForSeconds(0.1f);
        }

        // --- ASIL IŞINLANMA ---
        oyuncu.position += odaFarki;
        oda2deyim = !oda2deyim; // Odayı değiştir

        // --- ATMOSFER GÜNCELLEME ---
        if (oda2deyim)
        {
            // İkinci (Kanlı) Oda Ayarları
            if (kirmiziGokyuzu != null) RenderSettings.skybox = kirmiziGokyuzu;
            RenderSettings.fog = true;
            RenderSettings.fogColor = sisRengi;
            RenderSettings.fogDensity = sisYogunlugu;
        }
        else
        {
            // Birinci (Normal) Oda Ayarları
            if (normalGokyuzu != null) RenderSettings.skybox = normalGokyuzu;
            RenderSettings.fog = false; // Sisi kapat
        }

        DynamicGI.UpdateEnvironment();
        isinlanmaBasladi = false;
    }
}