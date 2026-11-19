using UnityEngine;

public class GeneradorElementos : MonoBehaviour
{
    [Header("Configuración de Elementos")]
    [SerializeField] private Transform[] puntosSpawn;
    [SerializeField] private GameObject[] prefabsMonedas;
    [SerializeField] private GameObject[] prefabsObstaculos;
    [SerializeField] private GameObject[] prefabsDinosaurios;

    [Header("Reglas de Generación")]
    [SerializeField] private float distanciaSegura = 40f;
    [SerializeField] private int maxDinosauriosActivos = 4;
    [SerializeField] private float tiempoEsperaDinosaurio = 2f;

    private static float ultimoTiempoSpawnDino = -10f;

    void Start()
    {
        GenerarUnSoloElemento();
    }

    private void GenerarUnSoloElemento()
    {
        if (puntosSpawn.Length == 0) return;

        int indiceCarril = Random.Range(0, puntosSpawn.Length);
        Transform puntoElegido = puntosSpawn[indiceCarril];

        int probabilidad = Random.Range(1, 101);
        GameObject prefabAElegir = null;

        int tipoElegido = 0; // 0=Nada, 1=Moneda, 2=Obstáculo, 3=Dinosaurio

        bool enZonaSegura = transform.position.z < distanciaSegura;

        if (probabilidad <= 15)
        {
            return; // 15% Vacío
        }
        else if (probabilidad > 15 && probabilidad <= 45 && prefabsMonedas.Length > 0)
        {
            prefabAElegir = prefabsMonedas[Random.Range(0, prefabsMonedas.Length)];
            tipoElegido = 1; // 30% Monedas
        }
        else if (probabilidad > 45 && probabilidad <= 75 && prefabsObstaculos.Length > 0)
        {
            if (enZonaSegura) return;

            prefabAElegir = prefabsObstaculos[Random.Range(0, prefabsObstaculos.Length)];
            tipoElegido = 2; // 30% Obstáculos
        }
        else if (probabilidad > 75 && prefabsDinosaurios.Length > 0)
        {
            if (enZonaSegura) return;

            if (EnemigoDinosaurio.dinosauriosActivos < maxDinosauriosActivos && (Time.time - ultimoTiempoSpawnDino) > tiempoEsperaDinosaurio)
            {
                prefabAElegir = prefabsDinosaurios[Random.Range(0, prefabsDinosaurios.Length)];
                ultimoTiempoSpawnDino = Time.time;
                tipoElegido = 3; // 25% Dinosaurios
            }
            else if (prefabsObstaculos.Length > 0)
            {
                prefabAElegir = prefabsObstaculos[Random.Range(0, prefabsObstaculos.Length)];
                tipoElegido = 2;
            }
        }

        if (prefabAElegir != null)
        {
            Quaternion rotacionFinal = puntoElegido.rotation;

            if (tipoElegido == 2)
            {
                float rotacionAleatoriaY = Random.Range(0, 4) * 90f;
                rotacionFinal = Quaternion.Euler(0, rotacionAleatoriaY, 0);
            }
            else if (tipoElegido == 3)
            {
                rotacionFinal = Quaternion.Euler(0, 180f, 0);
            }

            GameObject nuevoElemento = Instantiate(prefabAElegir, puntoElegido.position, rotacionFinal);
            nuevoElemento.transform.SetParent(transform);
        }
    }
}