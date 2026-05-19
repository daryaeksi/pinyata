using UnityEngine;

public class IntroBitirici : MonoBehaviour
{
    public GameObject sinematikKamera;
    public GameObject oyuncuKarakteri;

    // Bu fonksiyonu Timeline'dan "Event" olarak çağıracağız
    public void IntroBitti()
    {
        sinematikKamera.SetActive(false); // Kamerayı kapat
        oyuncuKarakteri.SetActive(true);  // Karakteri uyandır
        gameObject.SetActive(false);      // Kendini de kapat (iş bitti)
    }
}