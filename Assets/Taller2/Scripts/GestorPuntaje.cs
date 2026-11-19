using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GestorPuntaje : MonoBehaviour
{
    public static GestorPuntaje Instancia;

    [Header("Textos de la Interfaz Base")]
    [SerializeField] private TextMeshProUGUI textoPuntajeUI;
    [SerializeField] private TextMeshProUGUI textoMonedasUI;

    [Header("Pantalla Game Over")]
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private TextMeshProUGUI textoPuntajeFinalUI; // Texto para el puntaje de esta partida
    [SerializeField] private TextMeshProUGUI textoRecordUI;       // Texto para el High Score histórico

    [Header("Configuración")]
    [SerializeField] private float multiplicadorPuntaje = 100f;

    private int contadorMonedas = 0;
    private float puntajeDistancia = 0f;
    private bool jugadorVivo = true;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (textoMonedasUI != null) textoMonedasUI.text = "Monedas: 0";
        if (panelGameOver != null) panelGameOver.SetActive(false);
        ActualizarTextoPuntaje();
    }

    void Update()
    {
        if (jugadorVivo)
        {
            puntajeDistancia += Time.deltaTime * multiplicadorPuntaje;
            ActualizarTextoPuntaje();
        }
    }

    public void SumarMoneda(int cantidad)
    {
        contadorMonedas += cantidad;
        if (textoMonedasUI != null) textoMonedasUI.text = "Monedas: " + contadorMonedas;
    }

    public void SumarPuntosFijos(int puntosExtra)
    {
        puntajeDistancia += puntosExtra;
        ActualizarTextoPuntaje();
    }

    private void ActualizarTextoPuntaje()
    {
        if (textoPuntajeUI != null) textoPuntajeUI.text = "Puntaje: " + Mathf.FloorToInt(puntajeDistancia);
    }

    public void DetenerPuntaje()
    {
        jugadorVivo = false;

        // Convertimos el puntaje a número entero
        int puntajeFinal = Mathf.FloorToInt(puntajeDistancia);

        // --- LÓGICA DE PLAYERPREFS (GUARDADO) ---
        // 1. Buscamos el récord anterior. Si nunca has jugado, devolverá 0.
        int recordGuardado = PlayerPrefs.GetInt("RecordPuntaje", 0);

        // 2. Si el puntaje actual es mayor al récord, lo actualizamos
        if (puntajeFinal > recordGuardado)
        {
            recordGuardado = puntajeFinal;
            PlayerPrefs.SetInt("RecordPuntaje", recordGuardado);
            PlayerPrefs.Save(); // Guarda físicamente el dato en el disco
        }

        // 3. Mostramos los números en los textos de la pantalla de Game Over
        if (textoPuntajeFinalUI != null) textoPuntajeFinalUI.text = "Obtuviste: " + puntajeFinal;
        if (textoRecordUI != null) textoRecordUI.text = "Record: " + recordGuardado;

        // Encendemos el panel
        if (panelGameOver != null) panelGameOver.SetActive(true);
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        EnemigoDinosaurio.dinosauriosActivos = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}