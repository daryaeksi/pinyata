using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic; // Liste kullanımı için şart

public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;
    public float scaleMultiplier = 1.1f;
    public float duration = 0.15f;

    [Header("Ses Ayarları")]
    public AudioSource audioSource;
    public List<AudioClip> hoverSounds; // 4 tane hover sesini buraya ekleyeceğiz
    public AudioClip clickSound;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * scaleMultiplier, duration).SetUpdate(true);
        
        // Rastgele Hover Sesi Çalma
        if (hoverSounds != null && hoverSounds.Count > 0 && audioSource != null)
        {
            // Liste içinden rastgele bir index seç
            int randomIndex = Random.Range(0, hoverSounds.Count);
            audioSource.PlayOneShot(hoverSounds[randomIndex]);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, duration).SetUpdate(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}