using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Oyuncu Kontrolü")]
    [Tooltip("Boş bıraksan bile kod oyun başlarken otomatik bulacaktır")]
    public SimpleFPSController fpsKontrolcu; 

    [Header("Özel İmleç (Cursor)")]
    public Texture2D ozelImlec; 

    [Header("UI Panels")]
    public GameObject pauseMenuRoot;     
    public GameObject mainMenuContent;  
    public GameObject optionsContent;   
    public GameObject controlsContent;  
    public GameObject quitContent;      

    // Sadece Inspector'dan takip edebilmen için duruyor, kod artık buna bağımlı değil
    [HideInInspector] public bool isPaused = false;

    void Start()
    {
        // GÜVENLİK DUVARI: Eğer sürüklemeyi unuttuysan kod oyuncuyu otomatik bulur
        if (fpsKontrolcu == null)
        {
            fpsKontrolcu = Object.FindFirstObjectByType<SimpleFPSController>();
        }

        // Oyunu temiz bir şekilde başlat
        Resume(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // KESİN ÇÖZÜM: Değişkene değil, doğrudan panelin gerçek durumuna bakıyoruz!
            if (pauseMenuRoot != null && pauseMenuRoot.activeSelf) 
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }
    }

    public void StartGame()
    {
        Resume();
    }

    public void Resume()
    {
        if (pauseMenuRoot != null) pauseMenuRoot.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Fareyi oyun moduna sok
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        // FPS kontrolcüsünü tekrar aç (Kamera dönebilsin)
        if (fpsKontrolcu != null) fpsKontrolcu.enabled = true;
    }

    void Pause()
    {
        if (pauseMenuRoot != null) pauseMenuRoot.SetActive(true);
        ShowMainMenu(); 
        Time.timeScale = 0f; // Fizikleri ve zamanı durdurur
        isPaused = true;
        
        // Fareyi menü moduna sok
        Cursor.visible = true;
        
        // İŞLETİM SİSTEMİ ENGELİ: Fare oyun penceresinden dışarı (görev çubuğuna) çıkamaz
        Cursor.lockState = CursorLockMode.Confined;

        if (ozelImlec != null)
        {
            Cursor.SetCursor(ozelImlec, Vector2.zero, CursorMode.Auto);
        }

        // FPS kontrolcüsünü dondur (Kamera dönmeyi bıraksın ve fareyi çalmasın)
        if (fpsKontrolcu != null) fpsKontrolcu.enabled = false;
    }

    public void ShowMainMenu()
    {
        if (mainMenuContent != null) mainMenuContent.SetActive(true);
        if (optionsContent != null) optionsContent.SetActive(false);
        if (controlsContent != null) controlsContent.SetActive(false);
        if (quitContent != null) quitContent.SetActive(false); 
    }

    public void OpenOptions() { SetPanel(optionsContent); }
    public void OpenControls() { SetPanel(controlsContent); }
    public void OpenQuit() { SetPanel(quitContent); } 

    private void SetPanel(GameObject panelToOpen)
    {
        if (mainMenuContent != null) mainMenuContent.SetActive(false);
        if (optionsContent != null) optionsContent.SetActive(false);
        if (controlsContent != null) controlsContent.SetActive(false);
        if (quitContent != null) quitContent.SetActive(false); 
        
        if (panelToOpen != null) panelToOpen.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 

    public void QuitGame()
    {
        Application.Quit();
    }
}