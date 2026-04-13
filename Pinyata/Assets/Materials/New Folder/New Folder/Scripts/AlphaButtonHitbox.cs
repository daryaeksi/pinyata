using UnityEngine;
using UnityEngine.UI;

public class AlphaButtonHitbox : MonoBehaviour
{
    void Start()
    {
        // Hassasiyeti ayarlar (0.5f yarı şeffaf yerleri bile kapsar)
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }
}