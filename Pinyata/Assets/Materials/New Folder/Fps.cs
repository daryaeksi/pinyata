using UnityEngine;

public class FPSSabitleyici : MonoBehaviour
{
    [Header("Performans Ayarları")]
    [Range(30, 120)]
    public int hedefFPS = 60;

    [Header("Sinematik Ayarları")]
    public GameObject mainCamera;      // Senin asıl FPS kameran
    public GameObject cinematicCamera; // Timeline'ın kullandığı kamera
    public MonoBehaviour playerScript; // Karakterin yürüme scripti (buraya sürükle)

    void Awake()
    {
        // FPS Sabitleme
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = hedefFPS;
        Debug.Log("Sistem: FPS " + hedefFPS + " değerine sabitlendi.");
    }

    // Bu fonksiyonu Timeline bittiğinde çağıracağız
    public void OyunuBaslat()
    {
        // 1. Sinematik kamerayı kapat
        if (cinematicCamera != null) cinematicCamera.SetActive(false);

        // 2. Ana kamerayı aç
        if (mainCamera != null) mainCamera.SetActive(true);

        // 3. Karakterin yürüme kodunu aç
        if (playerScript != null) playerScript.enabled = true;

        // 4. Mouse imlecini oyun içine kilitle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Sinematik bitti, kontroller oyuncuya devredildi!");
    }
}