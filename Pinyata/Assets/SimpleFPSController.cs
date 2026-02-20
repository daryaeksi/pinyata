using UnityEngine;

public class SimpleFPSController : MonoBehaviour
{
    [Header("Hareket Ayarlari")]
    public float moveSpeed = 4f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;

    [Header("Kamera ve Sallanma")]
    public Transform playerCamera;
    [Range(0, 1)] public float swaySmoothing = 0.15f;
    public float idleSwaySpeed = 1.5f;
    public float idleSwayAmount = 0.03f;
    public float walkSwaySpeed = 10f;
    public float walkSwayAmount = 0.05f;

    [Header("Etkilesim Ayarlari")]
    public GameObject eButtonUI; 
    public Transform sofaSitPoint; 
    public float interactDistance = 4f; 
    public float sitHeightOffset = -0.6f; 
    public float sittingCameraHeight = 0.6f; 
    public float sittingYawLimit = 60f; 
    public string radioTag = "Radio"; 

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
        
        if(eButtonUI != null) eButtonUI.SetActive(false);
    }

    void Update()
    {
        if (isSitting)
        {
            transform.position = sofaSitPoint.position + new Vector3(0, sitHeightOffset, 0);
            transform.rotation = Quaternion.Euler(0f, sofaSitPoint.eulerAngles.y, 0f);
            if (Input.GetKeyDown(KeyCode.E)) StandUp();
            return;
        }

        HandleMovement();
        HandleInteraction();
    }

    void LateUpdate() { HandleRotationAndCamera(); }

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
            playerCamera.position = transform.position + new Vector3(0, sittingCameraHeight, 0);
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
                if (!hit.collider.CompareTag("SitTarget") && !hit.collider.CompareTag(radioTag)) 
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
        Vector3 targetPos = cameraDefaultLocalPos;

        if (isMoving) 
        {
            timer += Time.deltaTime * walkSwaySpeed;
            targetPos.y += Mathf.Sin(timer) * walkSwayAmount;
        } 
        else 
        {
            timer += Time.deltaTime * idleSwaySpeed;
            targetPos.y += Mathf.Sin(timer) * idleSwayAmount;
        }
        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, targetPos, swaySmoothing);
    }

    void HandleInteraction()
    {
        RaycastHit hit;
        
        // Işını görselleştirelim (Sadece Scene ekranında görünür)
        Debug.DrawRay(playerCamera.position, playerCamera.forward * interactDistance, Color.red);

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactDistance))
        {
            // KONSOLDA NEYE ÇARPTIĞINI GÖSTERİR (Sol alt köşeye bak!)
            Debug.Log("Çarpan Obje: " + hit.collider.gameObject.name + " | Tag: " + hit.collider.tag);

            if (hit.collider.CompareTag("SitTarget") || hit.collider.CompareTag(radioTag))
            {
                if (eButtonUI != null) eButtonUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    if (hit.collider.CompareTag("SitTarget")) 
                    {
                        SitDown();
                    }
                    else if (hit.collider.CompareTag(radioTag))
                    {
                        RadioController rc = hit.collider.GetComponent<RadioController>();
                        if(rc != null) rc.Interact();
                        else Debug.LogWarning("RadioController scripti bu objede bulunamadı!");
                    }
                }
            } 
            else 
            {
                if (eButtonUI != null) eButtonUI.SetActive(false);
            }
        } 
        else 
        {
            if (eButtonUI != null) eButtonUI.SetActive(false);
        }
    }

    void SitDown()
    {
        standPosition = transform.position; 
        isSitting = true; 
        controller.enabled = false;
        if (anim != null) anim.SetBool("IsSitting", true);
        sittingCenterYaw = sofaSitPoint.eulerAngles.y;
        yRotation = sittingCenterYaw;
    }

    void StandUp()
    {
        isSitting = false; 
        controller.enabled = true;
        transform.position = standPosition + Vector3.up * 0.1f;
        if (anim != null) anim.SetBool("IsSitting", false);
    }
}