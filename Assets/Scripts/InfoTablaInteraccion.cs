using UnityEngine;
using UnityEngine.UI;

// Detecta la Tabla Periodica con Raycast (igual que tus instrumentos),
// crea su propio texto contextual dentro de Canvas_HUD por codigo
// (porque Canvas_HUD se genera en tiempo de ejecucion desde SpawnManager.cs),
// y abre el panel de info con la tecla T.
//
// Se pone en un GameObject nuevo y vacio en la escena, por ejemplo "InfoTablaManager".
public class InfoTablaInteraccion : MonoBehaviour
{
    [Header("Configuracion de deteccion")]
    public float distanciaMaxima = 5f;
    public string tagTablaPeriodica = "TablaInfo";

    private Text textoContextual;
    private bool textoCreado = false;
    private bool estaMirandoLaTabla = false;

    void Update()
    {
        // Mientras no exista el Canvas_HUD todavia (por el orden de Start()),
        // intentamos crear el texto cada frame hasta lograrlo.
        if (!textoCreado)
        {
            IntentarCrearTexto();
            return;
        }

        // Si el panel ya esta abierto, no seguimos detectando ni mostrando el texto
        if (PanelTablaPeriodica.Instancia != null && PanelTablaPeriodica.Instancia.EstaAbierto())
        {
            ActualizarTexto(false);
            return;
        }

        DetectarTablaPeriodica();

        if (estaMirandoLaTabla && Input.GetKeyDown(KeyCode.T))
        {
            AbrirInfoTablaPeriodica();
        }
    }

    void IntentarCrearTexto()
    {
        GameObject canvasHUD = GameObject.Find("Canvas_HUD");
        if (canvasHUD == null) return; // Aun no existe, se reintenta el siguiente frame

        Font fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        GameObject textoGO = new GameObject("Texto_InfoTabla");
        textoGO.transform.SetParent(canvasHUD.transform, false);

        Text texto = textoGO.AddComponent<Text>();
        texto.font = fuente;
        texto.fontSize = 22;
        texto.color = Color.white;
        texto.alignment = TextAnchor.MiddleCenter;
        texto.fontStyle = FontStyle.Bold;

        Outline outline = textoGO.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(1.5f, -1.5f);

        RectTransform rt = textoGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(420f, 40f);
        // Un poco mas abajo que el texto de "[E] Agarrar/Soltar" (que esta en Y: -50)
        // para que no se encimen si algun dia coinciden en pantalla.
        rt.anchoredPosition = new Vector2(0f, -90f);

        textoGO.SetActive(false);

        textoContextual = texto;
        textoCreado = true;
    }

    void DetectarTablaPeriodica()
    {
        Camera camaraActiva = ObtenerCamaraActiva();
        if (camaraActiva == null)
        {
            estaMirandoLaTabla = false;
            ActualizarTexto(false);
            return;
        }

        Ray ray = new Ray(camaraActiva.transform.position, camaraActiva.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaMaxima))
        {
            if (hit.collider.CompareTag(tagTablaPeriodica))
            {
                estaMirandoLaTabla = true;
                ActualizarTexto(true);
                return;
            }
        }

        estaMirandoLaTabla = false;
        ActualizarTexto(false);
    }

    void ActualizarTexto(bool visible)
    {
        if (textoContextual == null) return;

        if (textoContextual.gameObject.activeSelf != visible)
            textoContextual.gameObject.SetActive(visible);

        if (visible)
            textoContextual.text = "[T] Ver Información: Tabla Periódica";
    }

    void AbrirInfoTablaPeriodica()
    {
        if (PanelTablaPeriodica.Instancia != null)
        {
            PanelTablaPeriodica.Instancia.AbrirPanel();
        }
        else
        {
            Debug.LogWarning("No se encontro PanelTablaPeriodica en la escena. " +
                              "Verifica que el GameObject 'PanelTablaManager' este creado.");
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