using UnityEngine;

/// <summary>
/// Instancia edificios aleatorios a los costados de cada plataforma para ambientar la ciudad.
/// </summary>
public class GeneradorEdificios : MonoBehaviour
{
    [Header("Configuración de Entorno")]
    [SerializeField] private Transform[] puntosEdificios;
    [SerializeField] private GameObject[] prefabsEdificios;

    [Range(0, 100)]
    [SerializeField] private int probabilidadEdificio = 85; // Deja un 15% de espacios vacíos o callejones

    void Start()
    {
        GenerarEntorno();
    }

    private void GenerarEntorno()
    {
        if (prefabsEdificios.Length == 0 || puntosEdificios.Length == 0) return;

        foreach (Transform punto in puntosEdificios)
        {
            if (Random.Range(0, 100) < probabilidadEdificio)
            {
                // Elige un edificio al azar de tu lista
                GameObject prefabElegido = prefabsEdificios[Random.Range(0, prefabsEdificios.Length)];

                // Lo crea usando la posición y rotación del punto (mirando hacia la calle)
                GameObject edificio = Instantiate(prefabElegido, punto.position, punto.rotation);

                // Lo hace hijo de la plataforma para que se mueva y se borre junto con ella
                edificio.transform.SetParent(transform);
            }
        }
    }
}