using UnityEngine;

public class PinataSallanma : MonoBehaviour
{
    [Header("Sallanma Ayarları")]
    [Tooltip("Pinyata ne kadar hızlı sallanacak?")]
    public float sallanmaHizi = 1.2f;
    
    [Tooltip("Pinyata ne kadar geniş bir açıyla sallanacak?")]
    public float sallanmaMiktari = 3.0f;
    
    [Tooltip("Rüzgarın düzensiz hissi (Farklı eksendeki sallanma çarpanı)")]
    public float ruzgarHissi = 0.6f;

    private Quaternion baslangicRotasyonu;

    void Start()
    {
        // Pinyatanın sahnedeki ilk duruş açısını hafızaya alıyoruz
        baslangicRotasyonu = transform.rotation;
    }

    void Update()
    {
        // Zaman akışını alıyoruz
        float zaman = Time.time;

        // Ana sallanma ekseni (Örneğin Z ekseninde sağa sola)
        // Mathf.Sin bize -1 ile 1 arasında yumuşak bir dalga verir
        float aciZ = Mathf.Sin(zaman * sallanmaHizi) * sallanmaMiktari;

        // Daha doğal durması için farklı bir eksende (örneğin X ekseni) daha yavaş/hızlı ikinci bir dalga
        float aciX = Mathf.Sin(zaman * (sallanmaHizi * 0.7f)) * (sallanmaMiktari * ruzgarHissi);

        // Hesaplanan açıları başlangıç açısının üzerine yumuşakça ekle
        Quaternion dalgalanma = Quaternion.Euler(aciX, 0, aciZ);
        transform.rotation = baslangicRotasyonu * dalgalanma;
    }
}