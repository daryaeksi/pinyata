using UnityEngine;

public class SimpleFPSController : MonoBehaviour
{
    [Header("Hareket Ayarlari")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 3f;
    public GameObject interactUI; 

    float yaw, pitch;
    Rigidbody rb;
    Camera cam;
    public Animator anim; // Inspector'dan X Bot'u buraya surukle
    bool isSitting = false;
    Transform currentSitPoint;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        
        // Eger Inspector'da atanmamissa otomatik bulmaya calis
        if (anim == null) anim = GetComponentInChildren<Animator>(); 

        rb.freezeRotation = true;
        
        // Fareyi ekrana kilitler, boylece karakter "odaklanir"
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Fare ile bakis kodlari
        yaw += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        
        if (isSitting)
        {
            float targetYaw = currentSitPoint.eulerAngles.y;
            yaw = Mathf.Clamp(yaw, targetYaw - 75f, targetYaw + 75f);
            cam.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            cam.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            
            // --- YURUME ANIMASYONU KONTROLU ---
            if (anim != null)
            {
                // Yatay hizi hesapla (Y eksenini dahil etme ki ziplarken animasyon bozulmasin)
                float horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
                float animationSpeed = horizontalVelocity / moveSpeed;
                
                // Gecis suresini (0.1f) sildik, boylece tus birakildigi an animasyon DURUR
                anim.SetFloat("Speed", animationSpeed);
            }

            CheckForInteractable();
        }

        if (currentSitPoint != null && Input.GetKeyDown(KeyCode.E))
        {
            if (!isSitting) Otur(); else Kalk();
        }
    }

    void FixedUpdate()
    {
        if (isSitting) return;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        
        if (move.magnitude > 0.1f) 
        {
            // Hareket varsa hizi uygula
            rb.linearVelocity = new Vector3(move.x * moveSpeed, rb.linearVelocity.y, move.z * moveSpeed);
        } 
        else 
        {
            // TUS BIRAKILDIGI AN HIZI SIFIRLA (Kaymayi ve isinlanmayi onler)
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); 
        }
    }

    void Otur()
    {
        isSitting = true; 
        rb.isKinematic = true; 
        transform.position = currentSitPoint.position;
        transform.rotation = currentSitPoint.rotation;
        if(anim != null) anim.Play("Sit"); 
    }

    void Kalk()
    {
        isSitting = false; 
        rb.isKinematic = false; 
        if(anim != null) anim.Play("Movement"); 
        transform.position += transform.forward * 0.8f;
    }

    void CheckForInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3f))
        {
            if (hit.collider.CompareTag("Couch"))
            {
                currentSitPoint = hit.collider.transform.Find("SitPoint");
                if (interactUI != null) interactUI.SetActive(true);
                return;
            }
        }
        currentSitPoint = null;
        if (interactUI != null) interactUI.SetActive(false);
    }
}