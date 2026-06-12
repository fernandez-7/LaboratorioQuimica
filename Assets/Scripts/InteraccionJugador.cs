using UnityEngine;
using UnityEngine.UI;

public class InteraccionJugador : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaInteraccion = 2f;
    public Transform puntoAgarre;
    public Camera camaraPrimeraPersona;
    public Camera camaraTerceraPersona;
    public Text textoInteraccion;

    private GameObject objetoAgarrado = null;

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
        ActualizarIndicador();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objetoAgarrado == null)
                IntentarAgarrar();
            else
                Soltar();
        }

        if (objetoAgarrado != null)
            objetoAgarrado.transform.position = puntoAgarre.position;
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
                Collider col = objetoAgarrado.GetComponent<Collider>();
                if (col != null) col.enabled = false;

                // Eliminar Rigidbody si existe para evitar conflicto con MeshCollider
                Rigidbody rb = objetoAgarrado.GetComponent<Rigidbody>();
                if (rb != null) Destroy(rb);
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
        Collider col = objetoAgarrado.GetComponent<Collider>();
        if (col != null) col.enabled = true;
        objetoAgarrado = null;
    }
}