using UnityEngine;

/// <summary>
/// Clase que gestiona la colisión de los obstáculos estáticos (autos, barricadas) con el jugador.
/// </summary>
public class Obstaculo : MonoBehaviour
{
    /// <summary>
    /// Detecta cuando otro objeto físico choca contra este obstáculo.
    /// </summary>
    /// <param name="collision">Datos de la colisión.</param>
    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que impactó tiene la etiqueta "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¡Choque fatal! El jugador ha muerto.");

            // Elimina al jugador de la escena
            Destroy(collision.gameObject);

            GestorPuntaje.Instancia.DetenerPuntaje();

            // Congela el tiempo para detener el avance del fondo
            Time.timeScale = 0f;
        }
    }
}