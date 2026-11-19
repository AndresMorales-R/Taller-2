using UnityEngine;

/// <summary>
/// Controla el movimiento del proyectil y su impacto contra enemigos.
/// </summary>
public class Bala : MonoBehaviour
{
    [SerializeField] private float velocidad = 25f;
    [SerializeField] private float tiempoDeVida = 3f;

    void Start()
    {
        // Destruye la bala automáticamente después de unos segundos 
        // si no choca con nada, para evitar saturar la memoria del juego.
        Destroy(gameObject, tiempoDeVida);
    }

    void Update()
    {
        // Mueve la bala hacia adelante en el eje Z local
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    /// <summary>
    /// Detecta cuando la bala atraviesa el colisionador de otro objeto.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo"))
        {
            EnemigoDinosaurio dino = other.GetComponent<EnemigoDinosaurio>();

            if (dino != null)
            {
                // Ahora le quita 1 vida en lugar de matarlo al instante
                dino.RecibirDano();
            }

            // La bala se destruye tras impactar, sin importar si el dinosaurio murió o no
            Destroy(gameObject);
        }
    }
}