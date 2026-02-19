using UnityEngine;

public class SimpleFPSController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 4f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;

    [Header("Kamera & Sallanma")]
    public Transform playerCamera;
    [Range(0, 1)] public float swaySmoothing = 0.15f;
    public float idleSwaySpeed = 1.5f;
    public float idleSwayAmount = 0.03f;
    public float walkSwaySpeed = 10f;
    public float walkSwayAmount = 0.05f;

    [Header("Etkileşim (Oturma)")]
    public GameObject eButtonUI; 
    public Transform sofaSitPoint; 
    public float interactDistance = 4f; 
    public float sitHeightOffset = -0.6f; 
    public float sittingCameraHeight = 0.6f; 
    public float sittingYawLimit = 60f; 
    
    private bool isSitting = false;
    private Vector3 standPosition;
    private CharacterController controller;
    private Animator anim; 
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float sittingCenterYaw; 
    private Vector3 velocity;
    
    private Vector3 cameraDefaultLocalPos;
    private float timer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>(); 
        Cursor.lockState = CursorLockMode.Locked;

        if (playerCamera == null) playerCamera = Camera.main.transform;
        cameraDefaultLocalPos = playerCamera.localPosition;
        yRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        if (isSitting)
        {
            // FİZİKSEL ÇAKIŞMAYI ENGELLE: Karakterin konumunu ve gövde rotasyonunu her karede zorla sabitle
            transform.position = sofaSitPoint.position + new Vector3(0, sitHeightOffset, 0);
            transform.rotation = Quaternion.Euler(0f, sofaSitPoint.eulerAngles.y, 0f);
            
            if (Input.GetKeyDown(KeyCode.E)) StandUp();
            return;
        }

        HandleMovement();
        HandleInteraction();
    }

    void LateUpdate()
    {
        HandleRotationAndCamera();
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
            // OTURURKEN: Gövde (transform) asla dönmez, sadece kamera (playerCamera) döner
            yRotation = Mathf.Clamp(yRotation, sittingCenterYaw - sittingYawLimit, sittingCenterYaw + sittingYawLimit);
            
            // Kamera pozisyonunu güncelle
            playerCamera.position = transform.position + new Vector3(0, sittingCameraHeight, 0);
            
            // Dönme hatasını sıfırlayan kritik hesaplama
            playerCamera.localRotation = Quaternion.Euler(xRotation, yRotation - sittingCenterYaw, 0f);
        }
        else
        {
            // AYAKTAYKEN
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Engel Kontrolü
            CheckObstacles();

            // Sway (Sallanma) Uygula
            ApplySway();
        }
    }

    void CheckObstacles()
    {
        bool isBlocked = false;
        float inputZ = Input.GetAxisRaw("Vertical");
        float inputX = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(inputZ) > 0.1f || Mathf.Abs(inputX) > 0.1f)
        {
            Vector3 moveDir = (transform.right * inputX + transform.forward * inputZ).normalized;
            Vector3 lowRay = transform.position + Vector3.up * 0.4f;
            Vector3 highRay = transform.position + Vector3.up * 1.2f;

            RaycastHit hit;
            if (Physics.Raycast(lowRay, moveDir, out hit, 0.8f) || Physics.Raycast(highRay, moveDir, out hit, 0.8f))
            {
                if (!hit.collider.CompareTag("SitTarget")) isBlocked = true;
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
        float inputX = Input.GetAxisRaw("Horizontal");
        bool isMoving = (inputZ != 0 || inputX != 0);

        Vector3 targetLocalPos = cameraDefaultLocalPos;
        if (isMoving && anim != null && anim.GetBool("IsWalking"))
        {
            timer += Time.deltaTime * walkSwaySpeed;
            targetLocalPos.y += Mathf.Sin(timer) * walkSwayAmount;
            targetLocalPos.x += Mathf.Cos(timer / 2) * (walkSwayAmount * 0.5f);
        }
        else
        {
            timer += Time.deltaTime * idleSwaySpeed;
            targetLocalPos.y += Mathf.Sin(timer) * idleSwayAmount;
        }
        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, targetLocalPos, swaySmoothing);
    }

    void HandleInteraction()
    {
        if (isSitting) return;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactDistance))
        {
            if (hit.collider.CompareTag("SitTarget"))
            {
                if (eButtonUI != null && !eButtonUI.activeSelf) eButtonUI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E)) SitDown();
            }
            else if (eButtonUI != null && eButtonUI.activeSelf) eButtonUI.SetActive(false);
        }
        else if (eButtonUI != null && eButtonUI.activeSelf) eButtonUI.SetActive(false);
    }

    void SitDown()
    {
        if (sofaSitPoint == null) return;
        standPosition = transform.position; 
        isSitting = true;
        controller.enabled = false; 

        transform.position = sofaSitPoint.position + new Vector3(0, sitHeightOffset, 0);
        transform.rotation = Quaternion.Euler(0f, sofaSitPoint.eulerAngles.y, 0f);
        
        sittingCenterYaw = sofaSitPoint.eulerAngles.y;
        yRotation = sittingCenterYaw; 
        xRotation = 0; 

        if (anim != null) 
        {
            anim.SetFloat("Vertical", 0f);
            anim.SetBool("IsSitting", true);
        }
        if (eButtonUI != null) eButtonUI.SetActive(false);
    }

    void StandUp()
    {
        isSitting = false;
        controller.enabled = true; 
        transform.position = standPosition + Vector3.up * 0.1f; 
        if (anim != null) anim.SetBool("IsSitting", false);
    }
}