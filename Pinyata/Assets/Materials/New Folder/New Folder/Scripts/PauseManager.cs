using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;

    [Header("UI Panels")]
    public GameObject pauseMenuRoot;     
    public GameObject mainMenuContent;  
    public GameObject optionsContent;   
    public GameObject controlsContent;  
    public GameObject quitContent;      

    void Start()
    {
        // Oyun başladığında ayarları sıfırla
        Time.timeScale = 1f;
        isPaused = false;
        
        if(pauseMenuRoot != null) pauseMenuRoot.SetActive(false);
        
        // Fareyi menü için görünür yap
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // Ana menüdeysen TAB tuşu çalışmasın (menü zaten açık)
        // Sadece oyun içindeyken (yani ana menü paneli kapalıyken) çalışsın
        if (mainMenuContent != null && mainMenuContent.activeInHierarchy) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    // NEW GAME BUTONU İÇİN
    public void StartGame()
    {
        // Panelleri kapat ve oyunu başlat
        if(pauseMenuRoot != null) pauseMenuRoot.SetActive(false);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void Resume()
    {
        pauseMenuRoot.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenuRoot.SetActive(true);
        ShowMainMenu(); 
        Time.timeScale = 0f;
        isPaused = true;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowMainMenu()
    {
        mainMenuContent.SetActive(true);
        optionsContent.SetActive(false);
        controlsContent.SetActive(false);
        quitContent.SetActive(false); 
    }

    // BUTONLARIN İÇİN GEREKLİ FONKSİYONLAR
    public void OpenOptions() { SetPanel(optionsContent); }
    public void OpenControls() { SetPanel(controlsContent); }
    public void OpenQuit() { SetPanel(quitContent); } 

    private void SetPanel(GameObject panelToOpen)
    {
        mainMenuContent.SetActive(false);
        optionsContent.SetActive(false);
        controlsContent.SetActive(false);
        quitContent.SetActive(false); 
        
        panelToOpen.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 

    // OYUNDAN ÇIKIŞ BUTONU İÇİN
    public void QuitGame()
    {
        Debug.Log("Oyundan çıkılıyor...");
        Application.Quit();
    }
}