using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject prefabNino;
    public GameObject prefabNina;
    public Transform puntoSpawn;

    void Start()
    {
        string personaje = SeleccionPersonaje.personajeElegido;
        GameObject prefabElegido;

        if (personaje == "Nina")
            prefabElegido = prefabNina;
        else
            prefabElegido = prefabNino;

        // Instanciar personaje en el punto de spawn
        GameObject jugador = Instantiate(
            prefabElegido,
            puntoSpawn.position,
            puntoSpawn.rotation
        );

        // Agregar componentes necesarios
        CharacterController cc = jugador.AddComponent<CharacterController>();
        cc.height = 1.8f;
        cc.center = new Vector3(0, 0.9f, 0);
        cc.radius = 0.3f;

        ControladorPersonaje cp = jugador.AddComponent<ControladorPersonaje>();

        // Crear cámara primera persona
        GameObject camFP = new GameObject("Camara_PrimeraPersona");
        camFP.transform.SetParent(jugador.transform);
        camFP.transform.localPosition = new Vector3(0, 1.6f, 0.1f);
        camFP.transform.localRotation = Quaternion.identity;
        Camera camaraFP = camFP.AddComponent<Camera>();
        camFP.AddComponent<AudioListener>();

        // Crear cámara tercera persona
        GameObject camTP = new GameObject("Camara_TerceraPersona");
        camTP.transform.SetParent(jugador.transform);
        camTP.transform.localPosition = new Vector3(0, 2f, -4f);
        camTP.transform.localRotation = Quaternion.Euler(15f, 0f, 0f);
        Camera camaraTP = camTP.AddComponent<Camera>();
        camaraTP.gameObject.SetActive(false);

        // Asignar cámaras al controlador
        cp.camaraPrimeraPersona = camaraFP;
        cp.camaraTerceraPersona = camaraTP;
    }
}