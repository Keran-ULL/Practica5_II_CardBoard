using UnityEngine;
using UnityEngine.InputSystem;   


public class MovimientoJugador : MonoBehaviour
{
    public Transform camara;                 
    public float velocidad = 2.0f;

    [Header("Caminar con la vista")]
    public bool caminarConLaVista = true;
    public float anguloCaminar = 25.0f;      
    public float anguloMaximo = 70.0f;       

    private float alturaSobreTerreno;

    void Start()
    {
        if (camara == null) camara = Camera.main.transform;

        Terrain t = Terrain.activeTerrain;
        if (t != null)
            alturaSobreTerreno = transform.position.y - AlturaTerreno(t, transform.position);
    }

    void Update()
    {
        Vector3 adelante = camara.forward;
        adelante.y = 0;
        adelante.Normalize();

        Vector3 derecha = camara.right;
        derecha.y = 0;
        derecha.Normalize();

        Vector2 entrada = Vector2.zero;

        // Mando
        if (Gamepad.current != null)
            entrada = Gamepad.current.leftStick.ReadValue();

        // Teclado 
        if (entrada == Vector2.zero && Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) entrada.y += 1;
            if (Keyboard.current.sKey.isPressed) entrada.y -= 1;
            if (Keyboard.current.dKey.isPressed) entrada.x += 1;
            if (Keyboard.current.aKey.isPressed) entrada.x -= 1;
        }

        Vector3 movimiento = adelante * entrada.y + derecha * entrada.x;

        if (entrada == Vector2.zero && caminarConLaVista)
        {
            float inclinacion = camara.eulerAngles.x;          
            if (inclinacion > 180) inclinacion -= 360;         
            if (inclinacion > anguloCaminar && inclinacion < anguloMaximo)
                movimiento = adelante;
        }

        if (movimiento.magnitude > 1) movimiento.Normalize();
        transform.position += movimiento * velocidad * Time.deltaTime;

        Terrain t = Terrain.activeTerrain;
        if (t != null)
        {
            Vector3 p = transform.position;
            p.y = AlturaTerreno(t, p) + alturaSobreTerreno;
            transform.position = p;
        }
    }

    float AlturaTerreno(Terrain t, Vector3 posicion)
    {
        return t.SampleHeight(posicion) + t.transform.position.y;
    }
}
