using UnityEngine;
using TMPro; // Fundamental para poder controlar el texto de la interfaz

/// <summary>
/// Controla la puntuación global del juego y actualiza la pantalla.
/// </summary>
public class GestorMoneda : MonoBehaviour
{
    // Patrón Singleton: Permite que otros scripts (como la moneda) se comuniquen con este fácilmente
    public static GestorMoneda Instancia;

    private int puntajeActual = 0;

    [SerializeField] private TextMeshProUGUI textoMonedaUI;

    void Awake()
    {
        // Configuración del Singleton
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ActualizarTexto();
    }

    /// <summary>
    /// Método público para que las monedas sumen puntos al ser recogidas.
    /// </summary>
    public void SumarPuntos(int puntos)
    {
        puntajeActual += puntos;
        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        if (textoMonedaUI != null)
        {
            textoMonedaUI.text = "Puntaje: " + puntajeActual;
        }
    }
}
