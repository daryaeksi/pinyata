using UnityEngine;
using UnityEngine.UI;

public class TVKarincalanma : MonoBehaviour
{
    public RawImage karincalanmaResmi;

    void Update()
    {
        // Resmi her saniye rastgele koordinatlara kaydırarak animasyon hissi yaratır
        if (karincalanmaResmi != null)
        {
            float x = Random.Range(0f, 1f);
            float y = Random.Range(0f, 1f);
            karincalanmaResmi.uvRect = new Rect(x, y, 1, 1);
        }
    }
}