using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Se pone sobre Instrumento_04 (el Vaso de Precipitado / punto de mezcla fijo).
// Ese objeto debe tener Tag "PuntoMezcla" y un Collider (ya lo tiene).
public class PuntoMezcla : MonoBehaviour
{
    [Header("Configuracion de deteccion")]
    public float distanciaMaxima = 3f;
    public string tagPuntoMezcla = "PuntoMezcla";

    [Header("Configuracion visual")]
    public float tiempoVisibleTrasPanel = 10f; // segundos que se ve la mezcla despues de cerrar el panel

    [Header("Referencias")]
    public Renderer liquidoRenderer;
    public BaseDeReacciones baseDeReacciones;

    private InteraccionJugador jugador;
    private Text textoContextual;
    private bool textoCreado = false;

    private List<Reactivo> reactivosVertidos = new List<Reactivo>();

    void Start()
    {
        jugador = FindObjectOfType<InteraccionJugador>();

        if (jugador == null)
        {
            Debug.LogWarning("PuntoMezcla: no se encontro InteraccionJugador en la escena.");
        }

        if (liquidoRenderer != null)
        {
            SetColorLiquido(new Color(1f, 1f, 1f, 0f));
        }
    }

    void Update()
    {
        if (!textoCreado)
        {
            IntentarCrearTexto();
            return;
        }

        // Si el panel de resultado esta abierto, no seguimos detectando
        if (PanelResultadoReaccion.Instancia != null && PanelResultadoReaccion.Instancia.EstaAbierto())
        {
            ActualizarTexto(false);
            return;
        }

        Reactivo reactivoEnMano = ObtenerReactivoEnMano();
        bool mirandoElVaso = EstaMirandoElVaso();

        if (mirandoElVaso && reactivoEnMano != null)
        {
            ActualizarTexto(true, reactivoEnMano.nombre);

            if (Input.GetKeyDown(KeyCode.F))
            {
                VerterReactivo(reactivoEnMano);
            }
        }
        else
        {
            ActualizarTexto(false);
        }
    }

    void VerterReactivo(Reactivo reactivo)
    {
        reactivosVertidos.Add(reactivo);
        jugador.DevolverObjetoAlOrigen();

        Color colorMezcla = reactivo.colorLiquido;
        if (reactivosVertidos.Count > 1)
        {
            colorMezcla = (reactivosVertidos[0].colorLiquido + reactivosVertidos[1].colorLiquido) / 2f;
        }
        SetColorLiquido(new Color(colorMezcla.r, colorMezcla.g, colorMezcla.b, 0.85f));

        Debug.Log($"Reactivo vertido: {reactivo.nombre}. Total en el vaso: {reactivosVertidos.Count}");

        if (reactivosVertidos.Count >= 2)
        {
            EvaluarReaccion();
        }
    }

    void EvaluarReaccion()
    {
        Reactivo a = reactivosVertidos[0];
        Reactivo b = reactivosVertidos[1];
        reactivosVertidos.Clear();

        ReaccionQuimica resultado = (baseDeReacciones != null) ? baseDeReacciones.BuscarReaccion(a, b) : null;

        if (resultado != null)
        {
            SetColorLiquido(new Color(resultado.colorResultado.r, resultado.colorResultado.g, resultado.colorResultado.b, 0.9f));

            if (PanelResultadoReaccion.Instancia != null)
            {
                PanelResultadoReaccion.Instancia.Mostrar(
                    resultado.nombreReaccion,
                    resultado.mensajeEducativo,
                    true,
                    () => StartCoroutine(LimpiarVasoConRetraso()) // al cerrar, espera y luego limpia
                );
            }
        }
        else
        {
            if (PanelResultadoReaccion.Instancia != null)
            {
                PanelResultadoReaccion.Instancia.Mostrar(
                    "Sin reacción",
                    $"{a.nombre} + {b.nombre} no produce ninguna reacción conocida. Intenta con otra combinación.",
                    false,
                    () => StartCoroutine(LimpiarVasoConRetraso())
                );
            }
        }
    }

    IEnumerator LimpiarVasoConRetraso()
    {
        yield return new WaitForSeconds(tiempoVisibleTrasPanel);
        SetColorLiquido(new Color(1f, 1f, 1f, 0f));
    }

    void SetColorLiquido(Color color)
    {
        if (liquidoRenderer == null) return;

        Material mat = liquidoRenderer.material;
        string[] propiedadesPosibles = { "_Color1", "_Color2", "_Color3", "_Color4", "_Color" };

        foreach (string propiedad in propiedadesPosibles)
        {
            if (mat.HasProperty(propiedad))
            {
                mat.SetColor(propiedad, color);
            }
        }
    }

    Reactivo ObtenerReactivoEnMano()
    {
        if (jugador == null || jugador.ObjetoAgarrado == null) return null;

        ContenedorQuimico contenedor = jugador.ObjetoAgarrado.GetComponent<ContenedorQuimico>();
        if (contenedor == null) return null;

        return contenedor.reactivoQueContiene;
    }

    bool EstaMirandoElVaso()
    {
        Camera cam = ObtenerCamaraActiva();
        if (cam == null) return false;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit[] golpes = Physics.RaycastAll(ray, distanciaMaxima);
        System.Array.Sort(golpes, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in golpes)
        {
            if (jugador != null && hit.collider.transform.IsChildOf(jugador.transform))
                continue;

            Transform t = hit.collider.transform;
            int niveles = 3;
            while (t != null && niveles > 0)
            {
                if (t.CompareTag(tagPuntoMezcla)) return true;
                t = t.parent;
                niveles--;
            }

            return false;
        }

        return false;
    }

    void IntentarCrearTexto()
    {
        GameObject canvasHUD = GameObject.Find("Canvas_HUD");
        if (canvasHUD == null) return;

        Font fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        GameObject textoGO = new GameObject("Texto_PuntoMezcla");
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
        rt.anchoredPosition = new Vector2(0f, -130f);

        textoGO.SetActive(false);

        textoContextual = texto;
        textoCreado = true;
    }

    void ActualizarTexto(bool visible, string nombreReactivo = "")
    {
        if (textoContextual == null) return;

        if (textoContextual.gameObject.activeSelf != visible)
            textoContextual.gameObject.SetActive(visible);

        if (visible)
            textoContextual.text = $"[F] Verter {nombreReactivo}";
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