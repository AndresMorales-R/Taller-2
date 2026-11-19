using UnityEngine;

public class Moneda : MonoBehaviour
{
    [SerializeField] private int valorMoneda = 1; // Cada moneda suma 1 al contador
    [SerializeField] private float velocidadRotacion = 100f;

    void Update()
    {
        transform.Rotate(0, velocidadRotacion * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ahora llama a SumarMoneda en lugar de SumarPuntos
            GestorPuntaje.Instancia.SumarMoneda(valorMoneda);
            Destroy(gameObject);
        }
    }
}