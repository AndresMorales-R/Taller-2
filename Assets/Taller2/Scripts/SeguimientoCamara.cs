using UnityEngine;

/// <summary>
/// Clase encargada de hacer que la cámara siga al jugador manteniéndose por arriba y detrás.
/// Cumple con la restricción de moverse únicamente en el eje Z (hacia adelante).
/// </summary>
public class SeguimientoCamara : MonoBehaviour
{
    /// <summary>
    /// Referencia al Transform del jugador que la cámara debe seguir.
    /// </summary>
    [SerializeField] private Transform jugador;

    /// <summary>
    /// Distancia relativa de la cámara respecto al jugador.
    /// X: Posición horizontal (debe ser 0 para estar centrada).
    /// Y: Altura (por arriba del jugador).
    /// Z: Distancia hacia atrás (debe ser un valor negativo).
    /// </summary>
    [SerializeField] private Vector3 compensacion = new Vector3(0f, 5f, -8f);

    /// <summary>
    /// Método de Unity ejecutado después de todos los Updates. 
    /// Ideal para actualizar cámaras sin generar temblores.
    /// </summary>
    void LateUpdate()
    {
        if (jugador != null)
        {
            // Mantiene fija la posición en X e Y según la compensación configurada.
            // Solo actualiza la posición en Z sumando la posición actual del jugador más el offset.
            Vector3 nuevaPosicion = new Vector3(compensacion.x, compensacion.y, jugador.position.z + compensacion.z);
            transform.position = nuevaPosicion;
        }
    }
}