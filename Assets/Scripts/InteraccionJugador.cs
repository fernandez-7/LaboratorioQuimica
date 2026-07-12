using UnityEngine;
using UnityEngine.UI;

public class InteraccionJugador : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaInteraccion = 2f;
    public Camera camaraPrimeraPersona;
    public Camera camaraTerceraPersona;

    [Header("UI")]
    public Text textoInteraccion;

    private GameObject objetoAgarrado = null;
    private Collider[] collidersAgarrado;
    private Transform padreOriginal;

    Camera ObtenerCamaraActiva()
    {
        if (camaraPrimeraPersona != null && camaraPrimeraPersona.gameObject.activeInHierarchy)
            return camaraPrimeraPersona;
        if (camaraTerceraPersona != null && camaraTerceraPersona.gameObject.activeInHierarchy)
            return camaraTerceraPersona;
        return Camera.main;
    }

    void Update()
    {
        // Verificar que el objeto agarrado sigue siendo válido
        if (objetoAgarrado != null && objetoAgarrado.transform.parent == null)
            objetoAgarrado = null;

        ActualizarIndicador();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objetoAgarrado == null)
                IntentarAgarrar();
            else
                Soltar();
        }
    }

    void ActualizarIndicador()
    {
        if (textoInteraccion == null) return;

        if (objetoAgarrado != null)
        {
            textoInteraccion.text = "[E] Soltar";
            textoInteraccion.gameObject.SetActive(true);
            return;
        }

        Camera cam = ObtenerCamaraActiva();
        if (cam == null) { textoInteraccion.gameObject.SetActive(false); return; }

        Ray rayo = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit golpe;

        if (Physics.Raycast(rayo, out golpe, distanciaInteraccion))
        {
            if (BuscarInteractuable(golpe.collider.gameObject) != null)
            {
                textoInteraccion.text = "[E] Agarrar";
                textoInteraccion.gameObject.SetActive(true);
                return;
            }
        }

        textoInteraccion.gameObject.SetActive(false);
    }

    void IntentarAgarrar()
    {
        if (objetoAgarrado != null) return;

        Camera cam = ObtenerCamaraActiva();
        if (cam == null) return;

        Ray rayo = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit golpe;

        if (Physics.Raycast(rayo, out golpe, distanciaInteraccion))
        {
            GameObject encontrado = BuscarInteractuable(golpe.collider.gameObject);
            if (encontrado != null)
            {
                objetoAgarrado = encontrado;
                padreOriginal = objetoAgarrado.transform.parent;

                // Limpiar componentes temporales de soltura anterior
                Rigidbody rbViejo = objetoAgarrado.GetComponent<Rigidbody>();
                if (rbViejo != null) Destroy(rbViejo);
                BoxCollider bcViejo = objetoAgarrado.GetComponent<BoxCollider>();
                if (bcViejo != null) Destroy(bcViejo);

                // Desactivar todos los colliders
                collidersAgarrado = objetoAgarrado.GetComponentsInChildren<Collider>(true);
                foreach (Collider col in collidersAgarrado)
                    col.enabled = false;

                // Siempre usar cámara primera persona para sostener el objeto
                Camera camAgarre = (camaraPrimeraPersona != null) ? camaraPrimeraPersona : cam;
                objetoAgarrado.transform.SetParent(camAgarre.transform);
                objetoAgarrado.transform.localPosition = new Vector3(0f, -0.3f, 0.8f);
                objetoAgarrado.transform.localRotation = Quaternion.identity;
            }
        }
    }

    GameObject BuscarInteractuable(GameObject obj)
    {
        Transform t = obj.transform;
        int niveles = 3;
        while (t != null && niveles > 0)
        {
            if (t.CompareTag("Interactuable")) return t.gameObject;
            t = t.parent;
            niveles--;
        }
        return null;
    }

    void Soltar()
    {
        if (objetoAgarrado == null) return;

        // Guardar posición mundial antes de desparentar
        Vector3 posicionMundial = objetoAgarrado.transform.position;

        objetoAgarrado.transform.SetParent(padreOriginal);
        objetoAgarrado.transform.position = posicionMundial;

        // Reactivar colliders originales
        if (collidersAgarrado != null)
        {
            foreach (Collider col in collidersAgarrado)
                col.enabled = true;
            collidersAgarrado = null;
        }

        // BoxCollider con tamaño adecuado para no atravesar superficies
        BoxCollider bc = objetoAgarrado.AddComponent<BoxCollider>();
        bc.size = new Vector3(0.15f, 0.15f, 0.15f);
        bc.center = Vector3.zero;

        // Rigidbody para la caída
        Rigidbody rb = objetoAgarrado.AddComponent<Rigidbody>();
        rb.mass = 0.3f;
        rb.drag = 1f;
        rb.angularDrag = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        objetoAgarrado = null;
    }
}