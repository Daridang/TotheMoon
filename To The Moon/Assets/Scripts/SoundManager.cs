using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioClip _collected;
    [SerializeField] private AudioClip _mainMenuBtns;

    void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    public void PlayCollectingSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(_collected, position);
    }

    public void PlayClickedSound()
    {
        AudioSource.PlayClipAtPoint(_mainMenuBtns, gameObject.transform.position, 0.2f);
    }
}
