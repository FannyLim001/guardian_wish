using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MusicPlayer");

        // Find the previous music GameObject.
        GameObject previousMusic = null;
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i] == this.gameObject)
            {
                continue;
            }

            previousMusic = objs[i];
            break;
        }

        // If there is a previous music GameObject, destroy it.
        if (previousMusic != null)
        {
            Destroy(previousMusic);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}






