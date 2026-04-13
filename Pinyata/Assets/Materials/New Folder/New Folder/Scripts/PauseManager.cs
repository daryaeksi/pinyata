using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool gameStarted = false; // Oyunun başlayıp başlamadığını tutar

    [Header("UI Panels")]
    public GameObject pauseMenuRoot;     
    public GameObject mainMenuContent;  
    public GameObject optionsContent;   
    public GameObject controlsContent;  

    void Start()
    {
        // Sahne her yüklendiğinde (Quit sonrası dahil) her şeyi sıfırla
        Time.timeScale = 1f;
        isPaused = false;
        gameStarted = false; // Başlangıçta ESC çalışmasın diye false yapıyoruz
        
        if(pauseMenuRoot != null) pauseMenuRoot.SetActive(false);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // SADECE oyun başladıysa ESC tuşu çalışsın
        if (gameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    // BU FONKSİYONU NEW GAME BUTONUNA BAĞLAYACAĞIZ
    public void StartGame()
    {
        gameStarted = true;
        Debug.Log("Oyun başladı, artık ESC ile Pause açılabilir.");
    }

    public void Resume()
    {
        pauseMenuRoot.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
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