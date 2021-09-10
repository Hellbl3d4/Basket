using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentificaPontos : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioS;
    [SerializeField]
    private AudioClip clip;
    
    public GameObject pontosImg;

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Bola") || col.gameObject.CompareTag("Bolaclone"))
        {
            GameManager.instance.Pontos++;

            GameManager.instance.moedasIntSave += GameManager.instance.Pontos * 50;

            UIManager.instance.moedasUI.text = (GameManager.instance.moedasIntSave).ToString("c");

            Shoot.fezPonto = true;
            TocaAudio.TocadordeAudio(audioS, clip);
            Instantiate(pontosImg, gameObject.transform.position, Quaternion.identity);
        }
    }
}
