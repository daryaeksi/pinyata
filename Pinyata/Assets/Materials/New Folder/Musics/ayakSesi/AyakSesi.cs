using UnityEngine;

public class AyakSesi : MonoBehaviour
{
    // Singleton yapısı: Diğer scriptlerden kolayca erişmek için
    public static AyakSesi instance;

    [Header("Gerekli Bileşenler")]
    public AudioSource sesKaynagi;
    public CharacterController karakterKontrolcu;
    
    [Header("Zemin Sesleri")]
    public AudioClip[] tahtaSesleri; 
    public AudioClip[] cimenSesleri; 

    [Header("Zamanlama Ayarları")]
    public float normalYurumeAraligi = 0.5f; 
    public float yavasYurumeAraligi = 0.9f;  
    
    private float yurumeAraligi;
    private float zamanlayici;
    private int sonCalinanIndex = -1;

    void Awake()
    {
        // Singleton kurulumu
        if (instance == null) instance = this;
    }

    void Start()
    {
        if (sesKaynagi == null) sesKaynagi = GetComponent<AudioSource>();
        if (karakterKontrolcu == null) karakterKontrolcu = GetComponent<CharacterController>();
        
        yurumeAraligi = normalYurumeAraligi;
    }

    void Update()
    {
        bool hareketTusuBasili = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

        if (hareketTusuBasili && karakterKontrolcu != null && karakterKontrolcu.isGrounded)
        {
            zamanlayici -= Time.deltaTime;

            if (zamanlayici <= 0f)
            {
                SesCal();
                zamanlayici = yurumeAraligi; 
            }
        }
        else
        {
            zamanlayici = 0f; 
        }
    }

    // BOŞLUK HATASI DÜZELTİLDİ
    public void SetAyakSesiHizi(bool yavasladiMi)
    {
        if (yavasladiMi)
        {
            yurumeAraligi = yavasYurumeAraligi; 
        }
        else
        {
            yurumeAraligi = normalYurumeAraligi; 
        }
    }

    private void SesCal()
    {
        AudioClip[] calinacakListe = HangiZemindeyiz();

        if (calinacakListe.Length > 0 && sesKaynagi != null)
        {
            int rastgeleIndex = Random.Range(0, calinacakListe.Length);
            
            if (calinacakListe.Length > 1) 
            {
                while (rastgeleIndex == sonCalinanIndex)
                {
                    rastgeleIndex = Random.Range(0, calinacakListe.Length);
                }
            }

            sonCalinanIndex = rastgeleIndex;
            sesKaynagi.pitch = Random.Range(0.95f, 1.05f); 
            sesKaynagi.PlayOneShot(calinacakListe[rastgeleIndex]);
        }
    }

    private AudioClip[] HangiZemindeyiz()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            if (hit.collider.CompareTag("Cimen")) return cimenSesleri;
            if (hit.collider.CompareTag("Tahta")) return tahtaSesleri;
        }
        return tahtaSesleri;
    }
}