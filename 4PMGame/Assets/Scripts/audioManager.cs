using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class audioManager : MonoBehaviour
{
    public AudioSource soundSource;
    public AudioSource BGMusic;

    public static audioManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartStopMusic();
    }

    public void playClip(AudioClip clip) {
        if(PlayerPrefs.GetInt("Sound", 1) == 1)
            soundSource.PlayOneShot(clip, 1.0f);
    }

    public void StartStopMusic()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            BGMusic.Play();
            BGMusic.loop = true;
        }
        else if(PlayerPrefs.GetInt("Sound") == 0)
        {
            BGMusic.Stop();
        }

    }
}
