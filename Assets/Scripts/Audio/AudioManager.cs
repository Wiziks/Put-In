using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _volumeSlider;
    public float MusicVolume { get; private set; }
    public float SoundVolume { get; private set; }
    private string musicVolumeSave = "MusicVolumeSave";
    private string soundVolumeSave = "SoundVolumeSave";

    [Header("Music")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioClip _musicInGame;
    [SerializeField] private AudioClip _musicInShop;
    [SerializeField] private AudioClip _musicInMenu;

    [Header("Sounds")]
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioClip _clickButton;
    [SerializeField] private AudioClip _buySound;
    [SerializeField] private AudioClip _notEnoughtMoneySound;
    [SerializeField] private AudioClip _takeDamageSource;
    [SerializeField] private AudioClip _breakWeaponSound;
    [SerializeField] private AudioClip _launchSound;
    [SerializeField] private AudioClip _explosionSound;
    public static AudioManager Instance;


    void Start()
    {
        Instance = this;
        if (PlayerPrefs.HasKey(musicVolumeSave))
            _musicSlider.value = PlayerPrefs.GetFloat(musicVolumeSave);
        else
            _musicSlider.value = 1f;

        if (PlayerPrefs.HasKey(soundVolumeSave))
            _volumeSlider.value = PlayerPrefs.GetFloat(soundVolumeSave);
        else
            _volumeSlider.value = 1f;

        MusicVolume = _musicSlider.value;
        SoundVolume = _volumeSlider.value;
        _musicSource.volume = MusicVolume;
        _soundSource.volume = SoundVolume;
        PlayMusicInMenu();
    }


    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
        _musicSource.volume = MusicVolume;
        PlayerPrefs.SetFloat(musicVolumeSave, MusicVolume);
        PlayerPrefs.Save();
    }

    public void SetSoundVolume(float value)
    {
        SoundVolume = value;
        _soundSource.volume = SoundVolume;
        PlayerPrefs.SetFloat(soundVolumeSave, SoundVolume);
        PlayerPrefs.Save();
    }

    public void PlayMusicInGame()
    {
        _musicSource.clip = _musicInGame;
        _musicSource.Play();
    }

    public void PlayMusicInShop()
    {
        _musicSource.clip = _musicInShop;
        _musicSource.Play();
    }

    public void PlayMusicInMenu()
    {
        _musicSource.clip = _musicInMenu;
        _musicSource.Play();
    }

    public void PlaySoundClickButton() { _soundSource.PlayOneShot(_clickButton); }
    public void PlaySoundBuy() { _soundSource.PlayOneShot(_buySound); }
    public void PlaySoundNEM() { _soundSource.PlayOneShot(_notEnoughtMoneySound); }
    public void PlaySoundDamage() { _soundSource.PlayOneShot(_takeDamageSource); }
    public void PlaySoundBreake() { _soundSource.PlayOneShot(_breakWeaponSound); }
    public void PlaySoundLaunch() { _soundSource.PlayOneShot(_launchSound); }
    public void PlaySoundExplosion() { _soundSource.PlayOneShot(_explosionSound); }

    public void StopMusic() { _musicSource.Pause(); }
    public void ContinueMusic() { _musicSource.UnPause(); }
}
