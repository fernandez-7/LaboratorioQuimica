using UnityEngine;

// Un Canvas en modo "World Space" necesita saber que camara lo esta "viendo"
// para que los clics del mouse funcionen sobre sus botones.
// Como el juego cambia entre camara de 1ra y 3ra persona (tecla Q),
// este script actualiza esa referencia constantemente.
//
// Se pone en el mismo GameObject que tiene el componente Canvas.
[RequireComponent(typeof(Canvas))]
public class AsignarCamaraCanvas : MonoBehaviour
{
    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        // Buscamos la camara activa en este momento (la que tiene el componente
        // Camera habilitado). Igual que ObtenerCamaraActiva() en InteraccionJugador.
        Camera camaraActiva = ObtenerCamaraActiva();

        if (camaraActiva != null && canvas.worldCamera != camaraActiva)
        {
            canvas.worldCamera = camaraActiva;
        }
    }

    Camera ObtenerCamaraActiva()
    {
        Camera[] camaras = Camera.allCameras;
        foreach (Camera cam in camaras)
        {
            if (cam.isActiveAndEnabled)
                return cam;
        }
        return null;
    }
}