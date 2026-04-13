using UnityEngine;

public class KapiKontrol : MonoBehaviour
{
    [Header("Hareket Ayarlari")]
    public bool acikMi = false;
    public float acilmaAcisi = -90f; // Çekmece ise bunu 0 bırakıp 'acilmaMesafesi' kullanacağız
    public float acilmaMesafesi = 0.5f; // Çekmece için ileri gitme miktarı
    public float hiz = 3f;
    public bool cekmeceMi = false; // Eğer bu bir çekmeceyse bunu Inspector'dan işaretle!

    private Vector3 kapaliPos;
    private Vector3 acikPos;
    private Quaternion kapaliRot;
    private Quaternion acikRot;

    void Start()
    {
        kapaliPos = transform.localPosition;
        kapaliRot = transform.localRotation;

        // Çekmece ve Kapı için ayrı hedef hesapla
        acikPos = kapaliPos + (Vector3.forward * acilmaMesafesi); // İleri doğru açılır
        acikRot = Quaternion.Euler(0, acilmaAcisi, 0) * kapaliRot;
    }

    void Update()
    {
        if (cekmeceMi)
        {
            Vector3 hedefPos = acikMi ? acikPos : kapaliPos;
            transform.localPosition = Vector3.Lerp(transform.localPosition, hedefPos, Time.deltaTime * hiz);
        }
        else
        {
            Quaternion hedefRot = acikMi ? acikRot : kapaliRot;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, hedefRot, Time.deltaTime * hiz);
        }
    }

    public void KapiyiAcKapat()
    {
        acikMi = !acikMi;
        Debug.Log(gameObject.name + " tetiklendi! Yeni durum: " + (acikMi ? "Açık" : "Kapalı"));
    }
}