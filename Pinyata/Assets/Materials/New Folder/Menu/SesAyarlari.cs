using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SesAyarlari : MonoBehaviour
{
    [Header("Gerekli Bileşenler")]
    public AudioMixer sesMixer;
    public Slider masterSlider; 
    public Slider muzikSlider;  
    public Slider sfxSlider;    

    void Start()
    {
        // GÜVENLİK RESETİ: Eğer hafıza bozulup 0'a kilitlendiyse, 
        // aşağıdaki satır tüm ses hafızasını bir kez temizler.
        // Sistem çalıştıktan sonra istersen başına // koyup kapatabilirsin.
        PlayerPrefs.DeleteAll(); 

        float kayitliMaster = PlayerPrefs.GetFloat("MasterSesi", 0.75f);
        float kayitliMuzik = PlayerPrefs.GetFloat("MuzikSesi", 0.75f);
        float kayitliSfx = PlayerPrefs.GetFloat("SfxSesi", 0.75f);

        // Gizlice slider konumlarını eşitle
        if (masterSlider != null) masterSlider.SetValueWithoutNotify(kayitliMaster);
        if (muzikSlider != null) muzikSlider.SetValueWithoutNotify(kayitliMuzik);
        if (sfxSlider != null) sfxSlider.SetValueWithoutNotify(kayitliSfx);

        // Miksere gönder
        MasterSesiAyarla(kayitliMaster);
        MuzikSesiAyarla(kayitliMuzik);
        SfxSesiAyarla(kayitliSfx);
    }

    public void MasterSesiAyarla(float sesSeviyesi)
    {
        float guvenliSes = Mathf.Clamp(sesSeviyesi, 0.0001f, 1f);
        if (sesMixer != null)
        {
            bool basarili = sesMixer.SetFloat("MasterVol", Mathf.Log10(guvenliSes) * 20);
            if (!basarili) Debug.LogWarning("DİKKAT: Mikserde 'MasterVol' isminde bir parametre bulunamadı!");
        }
        PlayerPrefs.SetFloat("MasterSesi", guvenliSes); 
    }

    public void MuzikSesiAyarla(float sesSeviyesi)
    {
        float guvenliSes = Mathf.Clamp(sesSeviyesi, 0.0001f, 1f);
        if (sesMixer != null)
        {
            bool basarili = sesMixer.SetFloat("MuzikVol", Mathf.Log10(guvenliSes) * 20);
            if (!basarili) Debug.LogWarning("DİKKAT: Mikserde 'MuzikVol' isminde bir parametre bulunamadı!");
        }
        PlayerPrefs.SetFloat("MuzikSesi", guvenliSes); 
    }

    public void SfxSesiAyarla(float sesSeviyesi)
    {
        float guvenliSes = Mathf.Clamp(sesSeviyesi, 0.0001f, 1f);
        if (sesMixer != null)
        {
            bool basarili = sesMixer.SetFloat("SFXVol", Mathf.Log10(guvenliSes) * 20);
            if (!basarili) Debug.LogWarning("DİKKAT: Mikserde 'SFXVol' isminde bir parametre bulunamadı!");
        }
        PlayerPrefs.SetFloat("SfxSesi", guvenliSes); 
    }
}