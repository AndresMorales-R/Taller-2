using UnityEngine;
using UnityEngine.SceneManagement; // Librería obligatoria para cambiar de escenas

public class ControladorMenu : MonoBehaviour
{
    // Asegúrate de escribir el nombre EXACTO de la escena de tu juego en el Inspector
    [SerializeField] private string nombreEscenaJuego = "SampleScene";

    /// <summary>
    /// Carga la escena principal del juego.
    /// </summary>
    public void EmpezarJuego()
    {
        // Restablecemos el tiempo por si veníamos de un Game Over pausado
        Time.timeScale = 1f;

        // Limpiamos los dinosaurios antes de empezar
        EnemigoDinosaurio.dinosauriosActivos = 0;

        SceneManager.LoadScene(nombreEscenaJuego);
    }

    /// <summary>
    /// Cierra la aplicación (Solo se nota al exportar el juego, no en el editor).
    /// </summary>
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}