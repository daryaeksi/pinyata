using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class IntroStart : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject cutsceneCamera;
    public GameObject mainCamera;
    public GameObject animCharacterIntro;
    public GameObject playerCharacter;
    public GameObject uiBlackImage;
    public PlayableDirector introTimeline;
    
    [Header("Zaman Ayarları")]
    public float transitionTime = 2f;
    public float introKacSaniyeSürsün = 5f; // BURAYA İSTEDİĞİN SANİYEYİ YAZABİLİRSİN

    private Animator uiAnimator;

    void Start()
    {
        if (uiBlackImage != null) uiAnimator = uiBlackImage.GetComponent<Animator>();
        
        mainMenuPanel.SetActive(true);
        playerCharacter.SetActive(false);
        mainCamera.SetActive(false);
        animCharacterIntro.SetActive(false);
        cutsceneCamera.SetActive(false);
    }

    public void ButonaBasildi()
    {
        StartCoroutine(IntroCutRoutine());
    }

    IEnumerator IntroCutRoutine()
    {
        if (uiAnimator != null) uiAnimator.SetTrigger("Play");

        yield return new WaitForSeconds(transitionTime / 2);
        
        mainMenuPanel.SetActive(false);
        animCharacterIntro.SetActive(true);
        cutsceneCamera.SetActive(true);

        if (introTimeline != null) introTimeline.Play();

        // ARTIK OTOMATİK SÜREYİ DEĞİL, SENİN YAZDIĞIN SANİYEYİ BEKLER
        yield return new WaitForSeconds(introKacSaniyeSürsün);

        if (uiAnimator != null) uiAnimator.SetTrigger("Play");

        yield return new WaitForSeconds(transitionTime / 2);

        animCharacterIntro.SetActive(false);
        cutsceneCamera.SetActive(false);

        playerCharacter.SetActive(true);
        mainCamera.SetActive(true);

        Debug.Log("Oyun Başladı!");
    }
}