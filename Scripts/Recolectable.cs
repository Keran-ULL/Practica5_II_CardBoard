using UnityEngine;


public class Recolectable : MonoBehaviour
{
    public float distanciaRecogida = 3.0f;     
    public float tiempoMirada = 1.0f;          
    public float velocidadAtraccion = 4.0f;    
    public static int totalRecogidos = 0;      

    private Transform jugador;                 
    private bool mirando = false;
    private bool atraido = false;
    private bool recogido = false;
    private float tiempoMirando = 0.0f;

    void Start()
    {
        jugador = Camera.main.transform;
    }

    void OnEnable()  { Recuperador.OnRecuperar += Atraer; }
    void OnDisable() { Recuperador.OnRecuperar -= Atraer; }

    public void OnPointerEnter()
    {
        mirando = true;
        tiempoMirando = 0.0f;
    }

    public void OnPointerExit()
    {
        mirando = false;
        tiempoMirando = 0.0f;
    }

    public void OnPointerClick()
    {
        if (EstaCerca()) Recoger();
    }

    void Update()
    {
        if (recogido) return;

        if (atraido)
        {
            transform.position = Vector3.MoveTowards(transform.position, jugador.position,
                                                     velocidadAtraccion * Time.deltaTime);
            if (Vector3.Distance(transform.position, jugador.position) < 1.0f)
                Recoger();
            return;
        }

        if (mirando && EstaCerca())
        {
            tiempoMirando += Time.deltaTime;
            if (tiempoMirando >= tiempoMirada)
                Recoger();
        }
    }

    bool EstaCerca()
    {
        return Vector3.Distance(transform.position, jugador.position) < distanciaRecogida;
    }

    void Atraer()
    {
        if (!recogido) atraido = true;
    }

    void Recoger()
    {
        recogido = true;
        totalRecogidos++;
        Debug.Log("Recogido " + gameObject.name + ". Total: " + totalRecogidos);

        foreach (Renderer r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        foreach (Collider c in GetComponentsInChildren<Collider>()) c.enabled = false;
    }
}
