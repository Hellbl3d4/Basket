using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGO : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioS;
    [SerializeField]
    private AudioClip clip;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bola") || col.gameObject.CompareTag("Bolaclone"))
        {
            TocaAudio.TocadordeAudio(audioS, clip);
        }
    }
}
