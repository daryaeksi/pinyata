using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool gameStarted = false; // Oyun başlayana kadar ESC çalışmasın

    [Header("UI Panels")]
    public GameObject pauseMenuRoot;     
    public GameObject mainMenuContent;  
    public GameObject optionsContent;   
    public GameObject controlsContent;  

    void Start()
    {
        // Oyun ilk açıldığında Ana Menüdeyiz. Zaman akıyor, ESC kapalı, FARE GÖRÜNÜR.
        Time.timeScale = 1f;
        isPaused = false;
        gameStarted = false; 
        
        if(pauseMenuRoot != null) pauseMenuRoot.SetActive(false);
        
        // Fareyi görünür yap (New Game butonuna basabilmek için)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // SADECE gameStarted "true" olduktan sonra ESC çalışacak
        if (gameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    // BU FONKSİYONU UNITY'DE "NEW GAME" BUTONUNA BAĞLAMALISIN
    public void StartGame()
    {
        gameStarted = true; // Artık ESC tuşu çalışabilir
        
        // Sinematik izlerken veya oyun oynarken farenin ekranda kalmaması için:
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // Ne olur ne olmaz zamanın aktığından emin olalım
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void Resume()
    {
        pauseMenuRoot.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Oyuna dönünce fareyi gizle ki karakteri/kamerayı yönetebil
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenuRoot.SetActive(true);
        ShowMainMenu();
        Time.timeScale = 0f;
        isPaused = true;
        
        // Menü açılınca fare görünür olsun ki butonlara tıklayabilelim
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowMainMenu()
    {
        mainMenuContent.SetActive(true);
        optionsContent.SetActive(false);
        controlsContent.SetActive(false);
    }

    public void OpenOptions() { SetPanel(optionsContent); }
    public void OpenControls() { SetPanel(controlsContent); }

    private void SetPanel(GameObject panelToOpen)
    {
        mainMenuContent.SetActive(false);
        optionsContent.SetActive(false);
        controlsContent.SetActive(false);
        panelToOpen.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        gameStarted = false; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}