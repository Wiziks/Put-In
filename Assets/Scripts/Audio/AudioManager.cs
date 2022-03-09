using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _volumeSlider;
    public float MusicVolume { get; private set; }
    public float SoundVolume { get; private set; }

    private string musicVolumeSave = "MusicVolumeSave";
    private string soundVolumeSave = "SoundVolumeSave";

    void Start()
    {
        if (PlayerPrefs.HasKey(musicVolumeSave))
            _musicSlider.value = PlayerPrefs.GetFloat(musicVolumeSave);
        else
            _musicSlider.value = 1f;

        if (PlayerPrefs.HasKey(soundVolumeSave))
            _volumeSlider.value = PlayerPrefs.GetFloat(soundVolumeSave);
        else
            _volumeSlider.value = 1f;
    }


    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
        PlayerPrefs.SetFloat(musicVolumeSave, MusicVolume);
        PlayerPrefs.Save();
    }

    public void SetSoundVolume(float value)
    {
        SoundVolume = value;
        PlayerPrefs.SetFloat(soundVolumeSave, SoundVolume);
        PlayerPrefs.Save();
    }
}
