using UnityEngine;

/// <summary>
/// Clase encargada de gestionar el movimiento principal del jugador, 
/// incluyendo su avance constante, desplazamiento lateral y saltos.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Velocidad a la que el jugador avanza hacia adelante de forma forzada.
    /// </summary>
    [SerializeField] private float velocidadAvance = 10f;

    /// <summary>
    /// Velocidad de desplazamiento lateral al usar las teclas A y D.
    /// </summary>
    [SerializeField] private float velocidadLateral = 7f;

    /// <summary>
    /// Límite en el eje X para restringir al jugador dentro de los tres carriles.
    /// </summary>
    [SerializeField] private float limiteLateralX = 4f;

    /// <summary>
    /// Fuerza del impulso aplicada hacia arriba al saltar.
    /// </summary>
    [SerializeField] private float fuerzaSalto = 7f;

    /// <summary>
    /// Referencia al componente de físicas del jugador.
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Variable booleana para evitar saltos infinitos en el aire.
    /// </summary>
    private bool enElSuelo = true;

    /// <summary>
    /// Método de Unity ejecutado al inicio. Inicializa las referencias.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Método de Unity ejecutado en cada frame para calcular la entrada y posición.
    /// </summary>
    void Update()
    {
        // 1. Movimiento hacia adelante forzado
        transform.Translate(Vector3.forward * velocidadAvance * Time.deltaTime);

        // 2. Movimiento lateral con "A" y "D"
        // Input.GetAxisRaw("Horizontal") devuelve -1 para A y 1 para D de forma nativa en Unity.
        float entradaX = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * entradaX * velocidadLateral * Time.deltaTime);

        // Limitar la posición lateral para no salir de los bordes del mapa (los 3 carriles)
        float posicionXRestringida = Mathf.Clamp(transform.position.x, -limiteLateralX, limiteLateralX);
        transform.position = new Vector3(posicionXRestringida, transform.position.y, transform.position.z);

        // 3. Salto hacia arriba (se asume la tecla Espacio por convención, o Flecha Arriba)
        // Solo permite saltar si el booleano indica que está tocando la plataforma.
        if (Input.GetKeyDown(KeyCode.Space) && enElSuelo)
        {
            // Aplica una fuerza instantánea (Impulse) en el eje Y
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            enElSuelo = false;
        }
    }

    /// <summary>
    /// Método de físicas de Unity invocado cuando este objeto colisiona con otro.
    /// </summary>
    /// <param name="collision">Datos del objeto contra el que se colisionó.</param>
    void OnCollisionEnter(Collision collision)
    {
        // Si el jugador vuelve a tocar la plataforma plana, se reactiva la capacidad de saltar.
        // Es necesario asignar el Tag "Suelo" al Prefab de la plataforma en Unity.
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enElSuelo = true;
        }
    }
}