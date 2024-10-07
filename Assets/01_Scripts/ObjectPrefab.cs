using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPrefab : MonoBehaviour
{
    public int value = 20;  
    public float speed = 0.3f;
    [SerializeField] private Puntuation pointsT;  // Asegúrate de que esto esté asignado en el Inspector

    private void Start()
    {
        // Encuentra el objeto que tiene el componente Puntuation en la escena
        pointsT = FindObjectOfType<Puntuation>();
    }
    void Update()
    {
        // Mueve el objeto hacia arriba
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si colisiona con el jugador, destruye el objeto y actualiza la puntuación
        if (collision.gameObject.CompareTag("Player"))
        {
            pointsT.UpdatePuntuation(value);  // Actualiza la puntuación
            Destroy(gameObject);  // Destruye el objeto
        }
    }
}

