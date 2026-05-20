using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject mainMenuRoot;    // Ana menü panelin
    public GameObject optionsMain;     // Yeni oluşturduğun Options menüsü
    
    [Header("Player")]
    public GameObject playerCharacter; 

    void Start()
    {
        // Oyun açıldığında sadece ana menü açık olsun, diğerleri kapalı
        mainMenuRoot.SetActive(true);
        optionsMain.SetActive(false); 
        
        if(playerCharacter != null) playerCharacter.SetActive(false);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        mainMenuRoot.SetActive(false);
        optionsMain.SetActive(false); // Oyun başlarken seçenekleri de kapat
        
        if(playerCharacter != null) playerCharacter.SetActive(true);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // OPTIONS butonuna bağlayacağın fonksiyon
    public void OpenOptions()
    {
        mainMenuRoot.SetActive(false); // Ana menüyü gizle
        optionsMain.SetActive(true);  // OptionsMain menüsünü aç
    }

    // Options menüsündeki GERİ (Back) butonuna bağlayacağın fonksiyon
    public void BackToMainMenu()
    {
        optionsMain.SetActive(false); // Options'ı kapat
        mainMenuRoot.SetActive(true); // Ana menüyü geri aç
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}