using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioClip _collected;
    [SerializeField] private AudioClip _mainMenuBtns;
    [SerializeField] private AudioSource _menu;
    [SerializeField] private AudioSource _game;
    [SerializeField] private float _bgmFadeDuration = .5f;

    public AudioMixer mixer;

    public AudioSetting[] audioSettings;

    private enum AudioGroups
    {
        Music,
        Sound
    };

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < audioSettings.Length; i++)
        {
            audioSettings[i].Initialize();
        }
    }

    public void SetMusicVolume(float value)
    {
        audioSettings[(int)AudioGroups.Music].SetExposedParam(value);
    }

    public void SetSFXVolume(float value)
    {
        audioSettings[(int)AudioGroups.Sound].SetExposedParam(value);
    }

    public void PlayCollectedSound(AudioSource audioSource)
    {
        audioSource.PlayOneShot(_collected);
    }

    public void PlayCollectingSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(_collected, position);
    }

    public void PlayClickedSound()
    {
        AudioSource.PlayClipAtPoint(_mainMenuBtns, gameObject.transform.position, 0.2f);
    }

    public void MenuToGame()
    {
        _game.Play();
        StartCoroutine(FadeMixerGroup.StartFade(
            mixer, "menu", _bgmFadeDuration, 0f));
        StartCoroutine(FadeMixerGroup.StartFade(
            mixer, "game", _bgmFadeDuration, PlayerPrefs.GetFloat("musicVol")));
    }

    public void GameToMenu()
    {
        StartCoroutine(FadeMixerGroup.StartFade(
            mixer, "game", _bgmFadeDuration, 0f));
        StartCoroutine(FadeMixerGroup.StartFade(
            mixer, "menu", _bgmFadeDuration, PlayerPrefs.GetFloat("musicVol")));
    }
}

[System.Serializable]
public class AudioSetting
{
    public Slider slider;
    public string exposedParam;

    public void Initialize()
    {
        slider.value = PlayerPrefs.GetFloat(exposedParam);
    }

    public void SetExposedParam(float value)
    {
        Debug.Log("WTF: " + exposedParam + " " + value);
        SoundManager.Instance.mixer.SetFloat(exposedParam, value);
        PlayerPrefs.SetFloat(exposedParam, value);
    }
}