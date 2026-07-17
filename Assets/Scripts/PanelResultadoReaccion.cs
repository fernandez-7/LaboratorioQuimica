using UnityEngine;
using UnityEngine.UI;

// Panel que aparece cuando el jugador vierte 2 reactivos en el vaso.
// Muestra el nombre de la reaccion (o "sin reaccion") y el mensaje educativo,
// pausando el juego mientras esta abierto. Al cerrar, ejecuta un callback
// (para que PuntoMezcla pueda limpiar SU vaso especifico, ya que puede haber
// varios PuntoMezcla en la escena, uno por mesa).
//
// Se pone en un GameObject nuevo y vacio, por ejemplo "PanelResultadoManager".
public class PanelResultadoReaccion : MonoBehaviour
{
    public static PanelResultadoReaccion Instancia { get; private set; }

    private GameObject panelRaiz;
    private Text textoTitulo;
    private Text textoMensaje;
    private Image fondoTitulo;
    private bool panelCreado = false;
    private bool panelAbierto = false;

    private System.Action alCerrarCallback;

    void Awake()
    {
        Instancia = this;
    }

    void Update()
    {
        if (panelAbierto && Input.GetKeyDown(KeyCode.Escape))
        {
            Cerrar();
        }
    }

    // exito = true -> se encontro una reaccion valida (fondo verde)
    // exito = false -> no hubo reaccion (fondo gris)
    public void Mostrar(string titulo, string mensaje, bool exito, System.Action alCerrar)
    {
        if (!panelCreado)
        {
            CrearPanel();
        }

        alCerrarCallback = alCerrar;

        textoTitulo.text = titulo;
        textoMensaje.text = mensaje;
        fondoTitulo.color = exito ? new Color(0.25f, 0.55f, 0.3f) : new Color(0.4f, 0.4f, 0.4f);

        panelRaiz.SetActive(true);
        panelAbierto = true;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Cerrar()
    {
        panelRaiz.SetActive(false);
        panelAbierto = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Avisamos a PuntoMezcla (el que abrio este panel) que ya puede
        // limpiar su vaso, ahora que el jugador termino de leer el resultado.
        alCerrarCallback?.Invoke();
        alCerrarCallback = null;
    }

    public bool EstaAbierto()
    {
        return panelAbierto;
    }

    void CrearPanel()
    {
        Font fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        panelRaiz = new GameObject("Canvas_PanelResultado");
        Canvas canvas = panelRaiz.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 60; // por encima de Canvas_HUD (10) y del panel de tabla (50)
        CanvasScaler scaler = panelRaiz.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        panelRaiz.AddComponent<GraphicRaycaster>();

        // Fondo oscuro semi-transparente
        GameObject fondoGO = new GameObject("Fondo");
        fondoGO.transform.SetParent(panelRaiz.transform, false);
        Image fondoImg = fondoGO.AddComponent<Image>();
        fondoImg.color = new Color(0f, 0f, 0f, 0.8f);
        RectTransform fondoRT = fondoGO.GetComponent<RectTransform>();
        fondoRT.anchorMin = Vector2.zero;
        fondoRT.anchorMax = Vector2.one;
        fondoRT.offsetMin = Vector2.zero;
        fondoRT.offsetMax = Vector2.zero;

        // Tarjeta central
        GameObject tarjetaGO = new GameObject("Tarjeta");
        tarjetaGO.transform.SetParent(panelRaiz.transform, false);
        Image tarjetaImg = tarjetaGO.AddComponent<Image>();
        tarjetaImg.color = new Color(0.95f, 0.95f, 0.95f, 1f);
        RectTransform tarjetaRT = tarjetaGO.GetComponent<RectTransform>();
        tarjetaRT.anchorMin = tarjetaRT.anchorMax = new Vector2(0.5f, 0.5f);
        tarjetaRT.sizeDelta = new Vector2(650f, 520f);
        tarjetaRT.anchoredPosition = Vector2.zero;

        // Barra de titulo (color cambia segun exito/fallo)
        GameObject tituloFondoGO = new GameObject("TituloFondo");
        tituloFondoGO.transform.SetParent(tarjetaGO.transform, false);
        fondoTitulo = tituloFondoGO.AddComponent<Image>();
        fondoTitulo.color = new Color(0.25f, 0.55f, 0.3f);
        RectTransform tituloFondoRT = tituloFondoGO.GetComponent<RectTransform>();
        tituloFondoRT.anchorMin = new Vector2(0f, 1f);
        tituloFondoRT.anchorMax = new Vector2(1f, 1f);
        tituloFondoRT.pivot = new Vector2(0.5f, 1f);
        tituloFondoRT.sizeDelta = new Vector2(0f, 90f);
        tituloFondoRT.anchoredPosition = Vector2.zero;

        // Texto del titulo (nombre de la reaccion)
        GameObject tituloGO = new GameObject("Titulo");
        tituloGO.transform.SetParent(tituloFondoGO.transform, false);
        textoTitulo = tituloGO.AddComponent<Text>();
        textoTitulo.font = fuente;
        textoTitulo.fontSize = 28;
        textoTitulo.fontStyle = FontStyle.Bold;
        textoTitulo.color = Color.white;
        textoTitulo.alignment = TextAnchor.MiddleCenter;
        RectTransform tituloRT = tituloGO.GetComponent<RectTransform>();
        tituloRT.anchorMin = Vector2.zero;
        tituloRT.anchorMax = Vector2.one;
        tituloRT.offsetMin = new Vector2(20f, 0f);
        tituloRT.offsetMax = new Vector2(-20f, 0f);

        // Texto del mensaje educativo
        GameObject mensajeGO = new GameObject("Mensaje");
        mensajeGO.transform.SetParent(tarjetaGO.transform, false);
        textoMensaje = mensajeGO.AddComponent<Text>();
        textoMensaje.font = fuente;
        textoMensaje.fontSize = 20;
        textoMensaje.color = new Color(0.2f, 0.2f, 0.2f);
        textoMensaje.alignment = TextAnchor.UpperLeft;
        textoMensaje.lineSpacing = 1.25f;
        textoMensaje.horizontalOverflow = HorizontalWrapMode.Wrap;
        textoMensaje.verticalOverflow = VerticalWrapMode.Overflow;
        RectTransform mensajeRT = mensajeGO.GetComponent<RectTransform>();
        mensajeRT.anchorMin = mensajeRT.anchorMax = new Vector2(0.5f, 0.5f);
        mensajeRT.pivot = new Vector2(0.5f, 0.5f);
        mensajeRT.anchoredPosition = new Vector2(0f, -25f); // ligeramente abajo del centro, para compensar la barra de titulo arriba
        mensajeRT.sizeDelta = new Vector2(580f, 260f);
        textoMensaje.alignment = TextAnchor.MiddleCenter;

        // Boton Cerrar
        GameObject cerrarGO = new GameObject("BotonCerrar");
        cerrarGO.transform.SetParent(tarjetaGO.transform, false);
        Image cerrarImg = cerrarGO.AddComponent<Image>();
        cerrarImg.color = new Color(0.2f, 0.4f, 0.75f);
        Button cerrarBtn = cerrarGO.AddComponent<Button>();
        cerrarBtn.onClick.AddListener(Cerrar);
        RectTransform cerrarRT = cerrarGO.GetComponent<RectTransform>();
        cerrarRT.anchorMin = cerrarRT.anchorMax = new Vector2(0.5f, 0f);
        cerrarRT.anchoredPosition = new Vector2(0f, 30f);
        cerrarRT.sizeDelta = new Vector2(160f, 45f);

        GameObject cerrarTextoGO = new GameObject("Texto");
        cerrarTextoGO.transform.SetParent(cerrarGO.transform, false);
        Text cerrarTexto = cerrarTextoGO.AddComponent<Text>();
        cerrarTexto.font = fuente;
        cerrarTexto.fontSize = 18;
        cerrarTexto.fontStyle = FontStyle.Bold;
        cerrarTexto.color = Color.white;
        cerrarTexto.alignment = TextAnchor.MiddleCenter;
        cerrarTexto.text = "Continuar";
        RectTransform cerrarTextoRT = cerrarTextoGO.GetComponent<RectTransform>();
        cerrarTextoRT.anchorMin = Vector2.zero;
        cerrarTextoRT.anchorMax = Vector2.one;
        cerrarTextoRT.offsetMin = Vector2.zero;
        cerrarTextoRT.offsetMax = Vector2.zero;

        panelRaiz.SetActive(false);
        panelCreado = true;
    }
}