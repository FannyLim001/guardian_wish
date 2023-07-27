using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMusic : MonoBehaviour
{
    public AudioSource music;

    void Awake()
    {
        // If the scene was loaded from a save file, load the music from the save file.
        if (PlayerPrefs.HasKey("Music"))
        {
            var audio = Resources.Load<AudioClip>(PlayerPrefs.GetString("Music"));
            music.clip = audio;
        }
    }

    void Start()
    {
        // If the music is not null, play it.
        if (SceneManager.GetActiveScene().name != "startScene")
        {
            music.Play();
        }
    }
}
