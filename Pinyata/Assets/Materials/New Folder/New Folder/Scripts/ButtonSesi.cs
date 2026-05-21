using UnityEngine;

public class ButonSesi : MonoBehaviour
{
    [Header("Ses Kaynağı")]
    public AudioSource sesKaynagi;

    [Header("Ses Efektleri")]
    public AudioClip uzerineGelmeSesi; 
    public AudioClip basildiSesi;       // Tuşa tıklandığı ilk milisaniyede çıkacak ses
    public AudioClip birakildiSesi;     // Parmak tuştan çekildiği an çıkacak ses

    // 1. Fare butonun üzerine gelince
    public void UzerineGelinceCal()
    {
        if (sesKaynagi != null && uzerineGelmeSesi != null)
        {
            sesKaynagi.PlayOneShot(uzerineGelmeSesi);
        }
    }

    // 2. Tuşa basıldığı an (Mouse Down)
    public void BasincaCal()
    {
        if (sesKaynagi != null && basildiSesi != null)
        {
            sesKaynagi.pitch = Random.Range(0.95f, 1.05f); // Sese çok hafif rastgelelik katar, mekanik hissi artırır
            sesKaynagi.PlayOneShot(basildiSesi);
        }
    }

    // 3. Tuş bırakıldığı an (Mouse Up)
    public void BirakincaCal()
    {
        if (sesKaynagi != null && birakildiSesi != null)
        {
            sesKaynagi.pitch = Random.Range(0.95f, 1.05f);
            sesKaynagi.PlayOneShot(birakildiSesi);
        }
    }
}