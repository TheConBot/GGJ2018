using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    AudioSource source;
    public static MusicManager instance { get; private set; }


    void Awake ()
    {

        source = GetComponent<AudioSource>();

        if (instance != null && instance != this)
        {
            // Destroy if another Gamemanager already exists
            Destroy(gameObject);
        }
        else
        {

            // Here we save our singleton S
            instance = this;
            // Furthermore we make sure that we don't destroy between scenes
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartSong(AudioClip song)
    {
        source.clip = song;
        source.Play();
    }
}
