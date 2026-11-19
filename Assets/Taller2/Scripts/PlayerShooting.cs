using UnityEngine;

/// <summary>
/// Clase encargada de gestionar la mecánica de disparo del jugador.
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    /// <summary>
    /// Prefabricado del proyectil (bala) que será instanciado al disparar.
    /// </summary>
    [SerializeField] private GameObject balaPrefab;

    /// <summary>
    /// Posición desde donde aparecerá el proyectil (por ejemplo, la punta del arma).
    /// </summary>
    [SerializeField] private Transform puntoDisparo;

    /// <summary>
    /// Intervalo mínimo de tiempo en segundos que debe pasar entre cada disparo.
    /// Según los requisitos, debe ser al menos 0.5 segundos.
    /// </summary>
    [SerializeField] private float tiempoEntreDisparos = 0.5f;

    /// <summary>
    /// Temporizador interno que almacena el momento exacto en el que se podrá volver a disparar.
    /// </summary>
    private float tiempoSiguienteDisparo = 0f;

    /// <summary>
    /// Método de Unity ejecutado en cada frame para detectar la entrada del jugador.
    /// </summary>
    void Update()
    {
        // Verifica si el jugador presiona la tecla "F" y si el tiempo actual ha superado el tiempo de enfriamiento (cooldown).
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= tiempoSiguienteDisparo)
        {
            Disparar();

            // Actualiza el temporizador sumando el intervalo de 0.5 segundos al tiempo actual del juego.
            tiempoSiguienteDisparo = Time.time + tiempoEntreDisparos;
        }
    }

    /// <summary>
    /// Método que instancia el proyectil en el entorno del juego.
    /// </summary>
    private void Disparar()
    {
        // Crea un clon del prefab de la bala en la posición y con la rotación del punto de disparo.
        if (balaPrefab != null && puntoDisparo != null)
        {
            Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);
        }
        else
        {
            Debug.LogWarning("Falta asignar el prefab de la bala o el punto de disparo en el Inspector de Unity.");
        }
    }
}