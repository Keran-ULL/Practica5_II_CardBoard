using UnityEngine;


public class Recuperador : MonoBehaviour
{
    public delegate void Mensaje();
    public static event Mensaje OnRecuperar;

    public float tiempoMirada = 1.5f;
    public Color colorMirando = Color.cyan;    

    private bool mirando = false;
    private float tiempoMirando = 0.0f;
    private Renderer[] renderers;
    private Color[] coloresOriginales;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        coloresOriginales = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            coloresOriginales[i] = renderers[i].material.color;
    }

    public void OnPointerEnter()
    {
        mirando = true;
        tiempoMirando = 0.0f;
        CambiarColor(true);
    }

    public void OnPointerExit()
    {
        mirando = false;
        CambiarColor(false);
    }

    public void OnPointerClick()
    {
        Activar();
    }

    void Update()
    {
        if (!mirando) return;

        tiempoMirando += Time.deltaTime;
        if (tiempoMirando >= tiempoMirada)
        {
            Activar();
            tiempoMirando = -999f;   
        }
    }

    void Activar()
    {
        Debug.Log("Recuperador activado: los recolectables vuelan hacia el jugador");
        OnRecuperar?.Invoke();
    }

    void CambiarColor(bool resaltar)
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = resaltar ? colorMirando : coloresOriginales[i];
    }
}
