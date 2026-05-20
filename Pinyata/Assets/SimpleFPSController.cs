using UnityEngine;

public class SimpleFPSController : MonoBehaviour
{
    [Header("Hareket Ayarlari")]
    public float moveSpeed = 4f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;

    [Header("Kamera ve Vucut Sallanma (Sway)")]
    public Transform playerCamera;
    public Transform bodyVisuals; 
    [Range(0, 1)] public float swaySmoothing = 0.15f;
    public float idleSwaySpeed = 1.5f;
    public float idleSwayAmount = 0.03f;
    public float walkSwaySpeed = 10f;
    public float walkSwayAmount = 0.05f;

    [Header("Etkilesim Ayarlari")]
    public GameObject eButtonUI; 
    public GameObject noktaImleci; 
    public Transform sofaSitPoint; 
    public float interactDistance = 5f; 
    public float npcBakmaMesafesi = 10f; 
    public float sitHeightOffset = -0.6f; 
    public float sittingCameraHeight = 0.6f; 
    public float sittingCameraForwardOffset = 0.2f; 
    public float sittingYawLimit = 60f; 
    public string radioTag = "Radio"; 
    public string kapiTag = "Kapi"; 
    public string npcTag = "NPC"; 

    // <--- YENİ: IŞINLANMA VE YAVAŞ ODA AYARLARI --->
    [Header("Gizli Oda Ayarlari")]
    public Transform gizliOdaNoktasi; // T'ye basınca gidilecek yer
    public float yavasOdaHizi = 1.5f; // Odadaki yavaş yürüme hızı
    private float normalHareketHizi; // Normal hızı hafızada tutmak için
    private bool gizliOdadaMi = false; 

    private bool isSitting = false;
    private Vector3 standPosition;
    private CharacterController controller;
    private Animator anim; 
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float sittingCenterYaw; 
    private Vector3 velocity;
    private Vector3 cameraDefaultLocalPos;
    private Vector3 bodyDefaultLocalPos; 
    private float timer = 0f;
    private bool kadinaBakildiMi = false;

    void Awake()
    {
        if(eButtonUI != null) eButtonUI.SetActive(false);
        if(noktaImleci != null) noktaImleci.SetActive(false);
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>(); 
        Cursor.lockState = CursorLockMode.Locked;
        if (playerCamera == null) playerCamera = Camera.main.transform;
        
        cameraDefaultLocalPos = playerCamera.localPosition;
        if (bodyVisuals != null) bodyDefaultLocalPos = bodyVisuals.localPosition; 

        yRotation = transform.eulerAngles.y;
        
        // Normal hızı oyun başlarken hafızaya alıyoruz
        normalHareketHizi = moveSpeed;

        if (anim != null) 
        {
            anim.SetBool("IsSitting", false);
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsSlowRoom", false); // Başlangıçta normal odadayız
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f) 
        {
            if (eButtonUI != null && eButtonUI.activeSelf) eButtonUI.SetActive(false);
            if (noktaImleci != null && noktaImleci.activeSelf) noktaImleci.SetActive(false);
            return; 
        }

        // <--- YENİ: T TUŞU İLE IŞINLANMA KONTROLÜ --->
        if (Input.GetKeyDown(KeyCode.T) && !isSitting)
        {
            TeleportToSecretRoom();
        }

        if (isSitting)
        {
            transform.position = sofaSitPoint.position + new Vector3(0, sitHeightOffset, 0);
            transform.rotation = Quaternion.Euler(0f, sofaSitPoint.eulerAngles.y, 0f);

            float ayarHizi = 0.5f; 
            if (Input.GetKey(KeyCode.UpArrow)) sittingCameraHeight += ayarHizi * Time.deltaTime;
            if (Input.GetKey(KeyCode.DownArrow)) sittingCameraHeight -= ayarHizi * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow)) sittingCameraForwardOffset += ayarHizi * Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow)) sittingCameraForwardOffset -= ayarHizi * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log($"<color=green><b>[KAMERA AYARI BULUNDU]</b></color> Inspector'a girilecek değerler -> " +
                          $"<b>Sitting Camera Height:</b> {sittingCameraHeight:F3} | " +
                          $"<b>Sitting Camera Forward Offset:</b> {sittingCameraForwardOffset:F3}");
            }

            if (Input.GetKeyDown(KeyCode.E)) StandUp();
            return; 
        }

        HandleMovement();
        HandleInteraction();
    }

    void LateUpdate() 
    { 
        if (Time.timeScale == 0f) return; 
        HandleRotationAndCamera(); 
    }

    // <--- YENİ: IŞINLANMA VE DURUM DEĞİŞTİRME FONKSİYONU --->
    void TeleportToSecretRoom()
    {
        if (gizliOdaNoktasi == null) 
        {
            Debug.LogWarning("Gizli oda noktasi (Transform) Inspector'da atanmamis!");
            return;
        }

        // Durumu tersine çevir (T'ye tekrar basarsan iptal olur mantığı için)
        gizliOdadaMi = !gizliOdadaMi;

        // Unity'de ışınlanma yaparken CharacterController'ı anlık kapatmak zorundayız
        controller.enabled = false;
        
        if (gizliOdadaMi)
        {
            transform.position = gizliOdaNoktasi.position; // Işınla
            moveSpeed = yavasOdaHizi; // Hızı düşür
        }
        else
        {
            // İstersen T'ye tekrar basınca eski yerine dönmesini sağlayabilirsin
            // Şimdilik sadece hızı ve animasyonu normale döndürüyor
            moveSpeed = normalHareketHizi; 
        }
        
        controller.enabled = true;

        // Animator'a yeni odaya girdiğimizi (veya çıktığımızı) haber veriyoruz
        if (anim != null)
        {
            anim.SetBool("IsSlowRoom", gizliOdadaMi);
        }
    }

    void HandleMovement()
    {
        if (controller == null || !controller.enabled || isSitting) return;
        
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1) move.Normalize();
        controller.Move(move * moveSpeed * Time.deltaTime);
        
        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleRotationAndCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        yRotation += mouseX;

        if (isSitting)
        {
            yRotation = Mathf.Clamp(yRotation, sittingCenterYaw - sittingYawLimit, sittingCenterYaw + sittingYawLimit);
            playerCamera.position = transform.position 
                                  + new Vector3(0, sittingCameraHeight, 0) 
                                  + (transform.forward * sittingCameraForwardOffset);
            playerCamera.localRotation = Quaternion.Euler(xRotation, yRotation - sittingCenterYaw, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            CheckObstacles();
            ApplySway();
        }
    }

    void CheckObstacles()
    {
        float inputZ = Input.GetAxisRaw("Vertical");
        float inputX = Input.GetAxisRaw("Horizontal");
        bool isBlocked = false;
        
        if (Mathf.Abs(inputZ) > 0.1f || Mathf.Abs(inputX) > 0.1f)
        {
            Vector3 moveDir = (transform.right * inputX + transform.forward * inputZ).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.4f, moveDir, out hit, 0.8f))
            {
                if (!hit.collider.CompareTag("SitTarget") && !hit.collider.CompareTag(radioTag) && !hit.collider.CompareTag(kapiTag)) 
                    isBlocked = true;
            }
        }
        
        if (anim != null) 
        {
            anim.SetFloat("Vertical", isBlocked ? 0f : inputZ);
            anim.SetBool("IsWalking", (inputZ != 0 || inputX != 0) && !isBlocked);
        }
    }

    void ApplySway()
    {
        float inputZ = Input.GetAxisRaw("Vertical");
        bool isMoving = (inputZ != 0 || Input.GetAxisRaw("Horizontal") != 0);
        
        float swayAmount = isMoving ? walkSwayAmount : idleSwayAmount;
        float swaySpeed = isMoving ? walkSwaySpeed : idleSwaySpeed;
        
        timer += Time.deltaTime * swaySpeed;
        float waveOffset = Mathf.Sin(timer) * swayAmount;

        Vector3 targetCamPos = cameraDefaultLocalPos;
        targetCamPos.y += waveOffset;
        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, targetCamPos, swaySmoothing);

        if (bodyVisuals != null)
        {
            Vector3 targetBodyPos = bodyDefaultLocalPos;
            targetBodyPos.y += waveOffset; 
            bodyVisuals.localPosition = Vector3.Lerp(bodyVisuals.localPosition, targetBodyPos, swaySmoothing);
        }
    }

    void HandleInteraction()
    {
        if (Time.timeScale == 0f) return;

        RaycastHit hit;
        bool bakilanKapiMi = false;
        bool bakilanDigerEtkilesimMi = false;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, npcBakmaMesafesi, ~LayerMask.GetMask("Ignore Raycast")))
        {
            if (hit.collider.CompareTag(npcTag))
            {
                if (!kadinaBakildiMi)
                {
                    AltyaziSistemi altyazi = FindObjectOfType<AltyaziSistemi>();
                    if (altyazi != null)
                    {
                        altyazi.AltyaziYaz("Baba - I'm home!");
                        kadinaBakildiMi = true;
                    }
                }
            }
            
            float mesafe = Vector3.Distance(playerCamera.position, hit.point);

            if (mesafe <= interactDistance)
            {
                if (hit.collider.CompareTag(kapiTag))
                {
                    bakilanKapiMi = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        KapiKontrol kk = hit.collider.GetComponentInParent<KapiKontrol>();
                        if (kk != null) kk.KapiyiAcKapat();
                    }
                }
                else if (hit.collider.CompareTag(radioTag) || hit.collider.CompareTag("SitTarget"))
                {
                    bakilanDigerEtkilesimMi = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (hit.collider.CompareTag("SitTarget")) SitDown();
                        else if (hit.collider.CompareTag(radioTag))
                        {
                            RadioController rc = hit.collider.GetComponent<RadioController>();
                            if (rc != null) rc.Interact();
                        }
                    }
                }
            }
        }

        if (noktaImleci != null) noktaImleci.SetActive(bakilanKapiMi);
        if (eButtonUI != null) eButtonUI.SetActive(bakilanKapiMi || bakilanDigerEtkilesimMi);
    }

    void SitDown() 
    { 
        standPosition = transform.position; 
        isSitting = true; 
        controller.enabled = false; 

        if (anim != null) 
        {
            anim.SetBool("IsWalking", false); 
            anim.SetBool("IsSitting", true);
        } 

        sittingCenterYaw = sofaSitPoint.eulerAngles.y; 
        yRotation = sittingCenterYaw; 
    }

    void StandUp() 
    { 
        isSitting = false; 
        controller.enabled = true; 
        transform.position = standPosition + Vector3.up * 0.1f; 

        if (anim != null) 
        {
            anim.SetBool("IsSitting", false);
        } 
    }
}