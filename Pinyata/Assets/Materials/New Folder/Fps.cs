using UnityEngine;

public class FPSSabitleyici : MonoBehaviour
{
    [Range(30, 120)] // Editörden kolayca kaydırarak değiştirebilmen için
    public int hedefFPS = 60;

    void Awake()
    {
        // 1. V-Sync'i kapatıyoruz ki bizim verdiğimiz hedefFPS baskın gelsin.
        QualitySettings.vSyncCount = 0;

        // 2. İşlemci ve ekran kartına "yavaşla" komutu veriyoruz.
        Application.targetFrameRate = hedefFPS;
        
        Debug.Log("Sistem: FPS " + hedefFPS + " değerine sabitlendi. Bilgisayar rahatladı!");
    }
}