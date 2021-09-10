using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMoedasSprite : MonoBehaviour
{
    private float vel = 2f;

    private void Update()
    {
        transform.Translate(Vector2.up * vel * Time.deltaTime); 
    }

    public void MorteMoedas()
    {
        Destroy(gameObject);
    }
}
