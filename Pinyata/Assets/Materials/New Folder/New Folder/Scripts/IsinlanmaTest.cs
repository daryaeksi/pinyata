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
    public Material normalGokyuzu; 
    public Color sisRengi = new Color(0.5f, 0, 0);
    public float sisYogunlugu = 0.05f;

    private bool oda2deyim = false; 
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

        // --- 1. FİZİĞİ GEÇİCİ OLARAK DURDUR ---
        // Eğer karakterde CharacterController varsa kapatıyoruz ki sapıtmasın
        CharacterController cc = oyuncu.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // Eğer karakterde Rigidbody varsa yerçekimini ve fiziği donduruyoruz
        Rigidbody rb = oyuncu.GetComponent<Rigidbody>();
        bool rbKinematicDurumu = false;
        if (rb != null) 
        {
            rbKinematicDurumu = rb.isKinematic;
            rb.isKinematic = true; // Karakteri %100 kodun kontrolüne (kinematik) alıyoruz
        }

        // Gidilecek yönü belirle
        Vector3 odaFarki = oda2deyim ? (oda1Merkez.position - oda2Merkez.position) : (oda2Merkez.position - oda1Merkez.position);

        // --- 2. GLITCH EFEKTİ ---
        for (int i = 0; i < 3; i++) 
        {
            oyuncu.position += odaFarki;
            yield return new WaitForSeconds(0.05f);
            oyuncu.position -= odaFarki;
            yield return new WaitForSeconds(0.1f);
        }

        // --- 3. ASIL IŞINLANMA VE YÜKSEKLİK AYARI ---
        Vector3 sonKonum = oyuncu.position + odaFarki;
        sonKonum.y += 0.5f; // Adamı zeminin içine saplanmasın diye hafif havadan bırakıyoruz
        oyuncu.position = sonKonum;
        
        oda2deyim = !oda2deyim;

        SimpleFPSController fpsController = oyuncu.GetComponent<SimpleFPSController>();
        if (fpsController != null)
            fpsController.OnRoomChanged(oda2deyim);

        // --- 4. ATMOSFER GÜNCELLEME ---
        if (oda2deyim)
        {
            if (kirmiziGokyuzu != null) RenderSettings.skybox = kirmiziGokyuzu;
            RenderSettings.fog = true;
            RenderSettings.fogColor = sisRengi;
            RenderSettings.fogDensity = sisYogunlugu;
        }
        else
        {
            if (normalGokyuzu != null) RenderSettings.skybox = normalGokyuzu;
            RenderSettings.fog = false; 
        }

        DynamicGI.UpdateEnvironment();

        // --- 5. FİZİĞİ GERİ AÇ ---
        // Işınlanma bittiği için kontrolleri karaktere geri veriyoruz
        if (cc != null) cc.enabled = true;
        if (rb != null) rb.isKinematic = rbKinematicDurumu;

        isinlanmaBasladi = false;
    }
}