using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TocaAudio : MonoBehaviour
{
    public static void TocadordeAudio(AudioSource Source, AudioClip clip)
    {
        Source.clip = clip;
        if(!Source.isPlaying)
        {
            Source.Play();
        }
    }
}
