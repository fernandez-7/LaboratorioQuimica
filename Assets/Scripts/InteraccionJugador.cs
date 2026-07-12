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
    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;

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
                posicionOriginal = objetoAgarrado.transform.position;
                rotacionOriginal = objetoAgarrado.transform.rotation;

                collidersAgarrado = objetoAgarrado.GetComponentsInChildren<Collider>(true);
                foreach (Collider col in collidersAgarrado)
                    col.enabled = false;

                objetoAgarrado.transform.SetParent(cam.transform);
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

        objetoAgarrado.transform.SetParent(padreOriginal);

        if (collidersAgarrado != null)
        {
            foreach (Collider col in collidersAgarrado)
                col.enabled = true;
            collidersAgarrado = null;
        }

        objetoAgarrado = null;
    }
}