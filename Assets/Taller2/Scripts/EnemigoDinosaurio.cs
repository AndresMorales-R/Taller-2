using UnityEngine;

public class EnemigoDinosaurio : MonoBehaviour
{
    [SerializeField] private float velocidadCorrer = 8f;
    [SerializeField] private float velocidadCaminar = 3f;

    [Header("Inteligencia Artificial")]
    [SerializeField] private float distanciaPersecucion = 25f;
    [SerializeField] private float distanciaAtaque = 3.5f;
    [SerializeField] private float velocidadRotacion = 5f;

    [Header("Estadísticas")]
    [SerializeField] private int vidas = 3;

    public static int dinosauriosActivos = 0;

    private Transform jugador;
    private bool yaPasoAlJugador = false;

    private Animator anim;
    private bool estaMuerto = false;

    // NUEVO: Variable para evitar que muerda 60 veces por segundo
    private bool yaAtaco = false;

    void Start()
    {
        dinosauriosActivos++;
        // Se asegura de buscar el Animator, ya sea en el objeto padre o en el modelo 3D hijo
        anim = GetComponentInChildren<Animator>();

        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null)
        {
            jugador = objJugador.transform;
        }
    }

    void OnDestroy()
    {
        if (dinosauriosActivos > 0 && !estaMuerto) dinosauriosActivos--;
    }

    void Update()
    {
        if (jugador == null || estaMuerto) return;

        if (!yaPasoAlJugador && transform.position.z < jugador.position.z)
        {
            yaPasoAlJugador = true;
        }

        if (yaPasoAlJugador)
        {
            anim.SetBool("Corriendo", true);
            transform.Translate(Vector3.forward * velocidadCorrer * Time.deltaTime);
            return;
        }

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaPersecucion)
        {
            anim.SetBool("Corriendo", true);

            // SOLUCIÓN AL ATAQUE: Solo dispara el Trigger si NO ha atacado antes
            if (distancia <= distanciaAtaque && !yaAtaco)
            {
                anim.SetTrigger("Atacar");
                yaAtaco = true;
            }

            Vector3 direccionObjetivo = (jugador.position - transform.position).normalized;
            direccionObjetivo.y = 0;

            RaycastHit hit;
            Vector3 origenRayo = transform.position + Vector3.up * 1f;

            if (Physics.Raycast(origenRayo, transform.forward, out hit, 5f))
            {
                if (hit.collider.CompareTag("Obstaculo"))
                {
                    direccionObjetivo += transform.right * 2.5f;
                }
            }

            if (direccionObjetivo != Vector3.zero)
            {
                Quaternion rotacionDeseada = Quaternion.LookRotation(direccionObjetivo);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * velocidadRotacion);
            }

            transform.Translate(Vector3.forward * velocidadCorrer * Time.deltaTime);
        }
        else
        {
            anim.SetBool("Corriendo", false);
            transform.Translate(Vector3.forward * velocidadCaminar * Time.deltaTime);
        }
    }

    public void RecibirDano()
    {
        if (estaMuerto) return;

        vidas--;
        if (vidas <= 0)
        {
            Morir();
        }
    }

    public void Morir()
    {
        if (estaMuerto) return;

        estaMuerto = true;

        // Obliga al Animator a saltar directamente al estado de muerte, ignorando flechas y Triggers
        // IMPORTANTE: Cambia "Morir" por el nombre EXACTO de la cajita en tu ventana Animator
        anim.Play("Morir");

        if (dinosauriosActivos > 0) dinosauriosActivos--;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 2.5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!estaMuerto && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¡El jugador ha muerto!");

            // Avisa al gestor para guardar el récord y encender el PanelGameOver
            if (GestorPuntaje.Instancia != null)
            {
                GestorPuntaje.Instancia.DetenerPuntaje();
            }

            Destroy(collision.gameObject);
            Time.timeScale = 0f;
        }
    }
}