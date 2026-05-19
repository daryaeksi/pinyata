using UnityEngine;

public class GelişmişRuzgarSallanmasi : MonoBehaviour
{
    [Header("Sallanma Ayarları")]
    public float ruzgarHizi = 1.0f; // Rüzgarın sertliği
    public float sallanmaAcisi = 12f; // X ve Z'de ne kadar savrulacağı
    
    [Header("Eksen Seçimi")]
    public bool xEksenindeSalla = true; 
    public bool zEksenindeSalla = true; // Tekinsiz bir daire çizmesi için Z'yi de açabilirsin

    [Header("Yüzerlik Ayarları (Y Ekseni)")]
    public bool yEksenindeYüzdür = true; // Bu yeni özellik!
    public float yuzmeHizi = 2.0f; // Yukarı-aşağı titreme hızı
    public float yuzmeMiktari = 0.1f; // Ne kadar yukarı-aşağı gideceği

    private Quaternion baslangicAcisi;
    private Vector3 baslangicPozisyonu;

    void Start()
    {
        // Pinyatanın oyuna başladığı ilk açı ve pozisyonu hafızaya al
        baslangicAcisi = transform.rotation;
        baslangicPozisyonu = transform.localPosition; // Child objesi olduğu için Local kullan
    }

    void Update()
    {
        // --- Sarkaç Sallanması (X ve Z) ---
        // Zaman ve Sinüs matematiği ile X ve Z için farklı dalgalar yaratıyoruz
        float dalgaX = Mathf.Sin(Time.time * ruzgarHizi) * sallanmaAcisi;
        float dalgaZ = Mathf.Sin(Time.time * ruzgarHizi * 1.3f) * (sallanmaAcisi * 0.7f); // Z'yi hafif farklılaştırıyoruz

        // Seçtiğimiz eksene göre pinyatayı salla
        float xSallanma = xEksenindeSalla ? dalgaX : 0f;
        float zSallanma = zEksenindeSalla ? dalgaZ : 0f;

        transform.rotation = baslangicAcisi * Quaternion.Euler(xSallanma, 0f, zSallanma);

        // --- Yüzerlik Etkisi (Y) ---
        if (yEksenindeYüzdür)
        {
            // Y ekseninde küçük, hızlı bir Sinüs dalgası ile yukarı-aşağı titreme yapıyoruz
            float yuzme = Mathf.Sin(Time.time * yuzmeHizi) * yuzmeMiktari;
            
            // Pinyatanın pozisyonuna sadece Y ekseninde yüzerlik ekliyoruz
            transform.localPosition = baslangicPozisyonu + new Vector3(0f, yuzme, 0f);
        }
    }
}