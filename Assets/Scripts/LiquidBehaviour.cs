using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiquidBehaviour : MonoBehaviour
{
    public float velocidadLiquido = 0.2f;

    void Update()
    {
        RawImage rawImage = GetComponent<RawImage>();
        rawImage.uvRect = new Rect(rawImage.uvRect.x + Time.deltaTime * velocidadLiquido, 0, 1, 1);
    }
}
