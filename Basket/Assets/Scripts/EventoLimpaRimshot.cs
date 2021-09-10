using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventoLimpaRimshot : MonoBehaviour
{
    public void LimpaRimshot()
    {
        GameManager.instance.rimShot = false;
    }

    public void LimpaSwishShot()
    {
        GameManager.instance.swishShot = false;
    }

    public void LimpaSkyHook()
    {
        GameManager.instance.skyHook = false;
    }
}
