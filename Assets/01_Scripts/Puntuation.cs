using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puntuation : MonoBehaviour
{
    private float points = 100f; // Inicializa los puntos a 0
    private TextMeshProUGUI texto;

    void Start()
    {
        texto = GetComponent<TextMeshProUGUI>(); // Obt�n el componente TextMeshProUGUI
        texto.text = points.ToString(); // Aseg�rate de que el texto se inicialice con el valor 0
    }

    void Update()
    {
        // El texto se actualiza seg�n el valor de points
        texto.text = points.ToString();
    }

    public void UpdatePuntuation(float v)
    {
        points += v; // Suma el valor al puntaje
    }
}
