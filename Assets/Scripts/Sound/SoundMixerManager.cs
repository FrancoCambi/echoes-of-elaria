using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider soundFXSlider;
    [SerializeField] Slider musicSlider;

    private void Start()
    {
        LoadVolumes();
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
        SaveMasterVolume();
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
        SaveSoundFXVolume();
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
        SaveMusicVolume();
    }

    private void SaveMasterVolume()
    {
        audioMixer.GetFloat("MasterVolume", out float masterVolume);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.Save();

    }

    private void SaveSoundFXVolume()
    {
        audioMixer.GetFloat("SoundFXVolume", out float soundFXVolume);
        PlayerPrefs.SetFloat("soundFXVolume", soundFXVolume);
        PlayerPrefs.Save();

    }

    private void SaveMusicVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    private void LoadVolumes()
    {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume");
        float soundFXVolume = PlayerPrefs.GetFloat("soundFXVolume");
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");

        float masterVolumeNormalized = Mathf.Pow(10, (masterVolume / (float)20));
        float soundFxVolumeNormalized = Mathf.Pow(10, (soundFXVolume / (float)20));
        float musicVolumeNormalized = Mathf.Pow(10, (musicVolume / (float)20));

        // When sliders' values are changed, the OnValueChanged event of the slider
        // sraises, calling the set functions.
        masterSlider.value = masterVolumeNormalized;
        soundFXSlider.value = soundFxVolumeNormalized;
        musicSlider.value = musicVolumeNormalized;
    }
}
