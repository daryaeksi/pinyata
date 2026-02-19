using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Sahne değiştirmek için gerekli kütüphane

public class GoToNextScene : MonoBehaviour
{
    [Header("Ayarlar")]
    // Timeline süren 55.33 olduğu için burayı 55.5 yapıyoruz
    public float beklemeSuresi = 10f; 
    
    // Buraya Project panelindeki Oyun sahnesinin ADINI birebir yazmalısın
    public string yuklenecekSahneAdi = "scene1"; 

    void Start()
    {
        // Oyun başlar başlamaz sayaç çalışmaya başlar
        StartCoroutine(NextSceneRoutine());
    }

    IEnumerator NextSceneRoutine()
    {
        // Belirlediğin saniye (55.5) kadar kod burada bekler
        yield return new WaitForSeconds(beklemeSuresi);

        // Sahneyi İSMİYLE yükler (Bu yöntem index hatasını çözer)
        if (!string.IsNullOrEmpty(yuklenecekSahneAdi))
        {
            SceneManager.LoadScene(yuklenecekSahneAdi);
        }
        else
        {
            Debug.LogError("HATA: yuklenecekSahneAdi kısmına bir isim yazmadın!");
        }
    }
}