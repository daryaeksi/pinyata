using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MusicSliderController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Image fillImage;
    
    [Header("Ayarlar")]
    public float hassasiyet = 20.0f;

    void Awake()
    {
        fillImage = GetComponent<Image>();
    }

    // Fareyle üzerine ilk tıklandığında çalışır
    public void OnPointerDown(PointerEventData eventData)
    {
        // Tıklandığı an bir işlem yapmamıza gerek yok ama bu interface şart
    }

    // Sadece bu objeyi sürüklediğinde çalışır
    public void OnDrag(PointerEventData eventData)
    {
        // Farenin o anki hareket hızını (Delta) alıyoruz
        // eventData.delta.x kullanımı Input.GetAxis'ten daha güvenlidir UI için
        float deltaX = eventData.delta.x;

        // Hassasiyete bölüp mevcut doluluğa ekle
        // Scale -1 olduğu için yön tersse (deltaX / hassasiyet) önüne - koy
        float newValue = fillImage.fillAmount + (deltaX / (hassasiyet * 100f));

        // 0-1 arasına hapset ve uygula
        fillImage.fillAmount = Mathf.Clamp01(newValue);

        Debug.Log(gameObject.name + " güncellendi: " + fillImage.fillAmount);
    }
}