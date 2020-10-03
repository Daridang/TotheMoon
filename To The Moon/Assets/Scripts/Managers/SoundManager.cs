using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioClip _collected;
    [SerializeField] private AudioClip _mainMenuBtns;
    [SerializeField] private AudioSource _clickSoundSource;
    [SerializeField] private AudioSource _menu;
    [SerializeField] private AudioSource _game;
    [SerializeField] private AudioSource _win;
    [SerializeField] private AudioSource _lose;
    [SerializeField] private string _menuBGM;
    [SerializeField] private string _gameBGM;
    [SerializeField] private string _winMusic;
    [SerializeField] private string _loseMusic;
    [SerializeField] private float _bgmFadeDuration = 0.6f;
    [SerializeField] private float crossFadeRate = 1.5f;

    public AudioMixer mixer;

    public AudioSetting[] audioSettings;

    private AudioSource _activeMusic;
    private AudioSource _inactiveMusic;
    private bool _crossFading;

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

        _activeMusic = _menu;
        _inactiveMusic = _game;
    }

    public void PlayMenuMusic()
    {
        PlayMusic(Resources.Load("Music/" + _menuBGM) as AudioClip, true);
    }

    public void PlayGameMusic()
    {
        PlayMusic(Resources.Load("Music/" + _gameBGM) as AudioClip, true);
    }
    
    public void PlayWinMusic()
    {
        PlayMusic(Resources.Load("Music/" + _winMusic) as AudioClip, false);
    }
    
    public void PlayLoseMusic()
    {
        PlayMusic(Resources.Load("Music/" + _loseMusic) as AudioClip, false);
    }

    private void PlayMusic(AudioClip clip, bool isLooping)
    {
        if (_crossFading) { return; }
        StartCoroutine(CrossFadeMusic(clip, isLooping));
    }

    private IEnumerator CrossFadeMusic(AudioClip clip, bool isLooping)
    {
        _crossFading = true;

        _inactiveMusic.clip = clip;
        _inactiveMusic.volume = 0;
        _inactiveMusic.Play();

        float scaledRate = crossFadeRate * 1f;
        while (_activeMusic.volume > 0)
        {
            _activeMusic.volume -= scaledRate * Time.deltaTime;
            _inactiveMusic.volume += scaledRate * Time.deltaTime;

            yield return null;
        }

        AudioSource temp = _activeMusic;

        _activeMusic = _inactiveMusic;
        _activeMusic.loop = isLooping;
        _activeMusic.volume = 1f;

        _inactiveMusic = temp;
        _inactiveMusic.Stop();

        _crossFading = false;
    }

    public void StopMusic()
    {
        _activeMusic.Stop();
        _inactiveMusic.Stop();
    }

    public void PlaySound(AudioClip clip)
    {
        _clickSoundSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float value)
    {
        audioSettings[(int)AudioGroups.Music].SetExposedParam(value);
    }

    public void SetSFXVolume(float value)
    {
        audioSettings[(int)AudioGroups.Sound].SetExposedParam(value);
    }

    public void PlayClickedSound()
    {
        AudioSource.PlayClipAtPoint(_mainMenuBtns, gameObject.transform.position, 0.2f);
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
        SoundManager.Instance.mixer.SetFloat(exposedParam, value);
        PlayerPrefs.SetFloat(exposedParam, value);
    }
}