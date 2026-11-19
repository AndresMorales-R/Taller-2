using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase encargada de gestionar la creación y destrucción de plataformas (suelos) 
/// para simular un camino infinito por delante del jugador.
/// </summary>
public class GeneradorSuelos : MonoBehaviour
{
    /// <summary>
    /// Prefabricado de la plataforma que se instanciará consecutivamente.
    /// </summary>
    [SerializeField] private GameObject prefabSuelo;

    /// <summary>
    /// Referencia al Transform del jugador para conocer su posición actual.
    /// </summary>
    [SerializeField] private Transform jugador;

    /// <summary>
    /// Cantidad de plataformas que se generarán al iniciar la partida. 
    /// El taller exige entre 10 y 30.
    /// </summary>
    [SerializeField] private int cantidadSuelosInicial = 15;

    /// <summary>
    /// Longitud en el eje Z de cada plataforma (debe coincidir con la escala Z del Prefab).
    /// </summary>
    [SerializeField] private float longitudSuelo = 20f;

    /// <summary>
    /// Posición en el eje Z donde se instanciará la siguiente plataforma.
    /// </summary>
    private float zSpawn = -15f;

    /// <summary>
    /// Cola (Queue) que almacena las plataformas activas en la escena para destruirlas en orden.
    /// </summary>
    private Queue<GameObject> suelosActivos = new Queue<GameObject>();

    /// <summary>
    /// Método de Unity ejecutado al inicio. Genera el camino inicial.
    /// </summary>
    void Start()
    {
        // Genera entre 10 y 30 plataformas al comenzar la partida.
        for (int i = 0; i < cantidadSuelosInicial; i++)
        {
            GenerarSuelo();
        }
    }

    /// <summary>
    /// Método de Unity ejecutado en cada frame para evaluar si se deben reciclar plataformas.
    /// </summary>
    void Update()
    {
        // VALIDADOR DE SEGURIDAD: Si el jugador fue destruido (es null), detenemos la ejecución de este método.
        if (jugador == null)
        {
            return;
        }

        // Si el jugador sigue vivo, continuamos evaluando la generación de suelo.
        if (jugador.position.z > (suelosActivos.Peek().transform.position.z + (longitudSuelo * 3)))
        {
            GenerarSuelo();
            DestruirSueloAntiguo();
        }
    }

    /// <summary>
    /// Instancia una nueva plataforma al final del camino y actualiza el punto de aparición.
    /// </summary>
    private void GenerarSuelo()
    {
        // Instancia el Prefab en la posición actual de zSpawn.
        GameObject nuevoSuelo = Instantiate(prefabSuelo, new Vector3(0, 0, zSpawn), Quaternion.identity);

        // Lo añade a la cola de suelos activos.
        suelosActivos.Enqueue(nuevoSuelo);

        // Mueve el punto de spawn hacia adelante para la próxima plataforma.
        zSpawn += longitudSuelo;
    }

    /// <summary>
    /// Elimina la plataforma más antigua por la que el jugador ya pasó.
    /// </summary>
    private void DestruirSueloAntiguo()
    {
        // Saca la primera plataforma de la cola (la más antigua) y la destruye.
        GameObject sueloEliminar = suelosActivos.Dequeue();
        Destroy(sueloEliminar);
    }
}