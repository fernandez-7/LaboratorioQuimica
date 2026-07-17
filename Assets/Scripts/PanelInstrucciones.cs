using UnityEngine;
using UnityEngine.UI;

// Panel de instrucciones: aparece automaticamente al iniciar el juego,
// y se puede volver a abrir en cualquier momento con la tecla H.
// Pausa el juego mientras esta abierto, igual que los demas paneles.
//
// Se pone en un GameObject nuevo y vacio, por ejemplo "PanelInstruccionesManager".
public class PanelInstrucciones : MonoBehaviour
{
    public static PanelInstrucciones Instancia { get; private set; }

    private GameObject panelRaiz;
    private bool panelCreado = false;
    private bool panelAbierto = false;

    void Awake()
    {
        Instancia = this;
    }

    void Start()
    {
        CrearPanel();
        Abrir(); // se muestra automaticamente al iniciar el juego
    }

    void Update()
    {
        if (!panelAbierto && Input.GetKeyDown(KeyCode.H))
        {
            if (OtroPanelAbierto()) return; // evita encimar paneles
            Abrir();
        }
        else if (panelAbierto && Input.GetKeyDown(KeyCode.Escape))
        {
            Cerrar();
        }
    }

    bool OtroPanelAbierto()
    {
        bool tablaAbierta = PanelTablaPeriodica.Instancia != null && PanelTablaPeriodica.Instancia.EstaAbierto();
        bool resultadoAbierto = PanelResultadoReaccion.Instancia != null && PanelResultadoReaccion.Instancia.EstaAbierto();
        return tablaAbierta || resultadoAbierto;
    }

    public void Abrir()
    {
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
    }

    public bool EstaAbierto()
    {
        return panelAbierto;
    }

    void CrearPanel()
    {
        if (panelCreado) return;

        Font fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        panelRaiz = new GameObject("Canvas_PanelInstrucciones");
        Canvas canvas = panelRaiz.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 70; // el mas alto de todos los paneles
        CanvasScaler scaler = panelRaiz.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        panelRaiz.AddComponent<GraphicRaycaster>();

        // Fondo oscuro
        GameObject fondoGO = new GameObject("Fondo");
        fondoGO.transform.SetParent(panelRaiz.transform, false);
        Image fondoImg = fondoGO.AddComponent<Image>();
        fondoImg.color = new Color(0f, 0f, 0f, 0.85f);
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
        tarjetaRT.sizeDelta = new Vector2(700f, 560f);
        tarjetaRT.anchoredPosition = Vector2.zero;

        // Barra de titulo
        GameObject tituloFondoGO = new GameObject("TituloFondo");
        tituloFondoGO.transform.SetParent(tarjetaGO.transform, false);
        Image tituloFondo = tituloFondoGO.AddComponent<Image>();
        tituloFondo.color = new Color(0.2f, 0.35f, 0.6f);
        RectTransform tituloFondoRT = tituloFondoGO.GetComponent<RectTransform>();
        tituloFondoRT.anchorMin = new Vector2(0f, 1f);
        tituloFondoRT.anchorMax = new Vector2(1f, 1f);
        tituloFondoRT.pivot = new Vector2(0.5f, 1f);
        tituloFondoRT.sizeDelta = new Vector2(0f, 80f);
        tituloFondoRT.anchoredPosition = Vector2.zero;

        GameObject tituloGO = new GameObject("Titulo");
        tituloGO.transform.SetParent(tituloFondoGO.transform, false);
        Text titulo = tituloGO.AddComponent<Text>();
        titulo.font = fuente;
        titulo.fontSize = 28;
        titulo.fontStyle = FontStyle.Bold;
        titulo.color = Color.white;
        titulo.alignment = TextAnchor.MiddleCenter;
        titulo.text = "Cómo Jugar — Laboratorio de Química";
        RectTransform tituloRT = tituloGO.GetComponent<RectTransform>();
        tituloRT.anchorMin = Vector2.zero;
        tituloRT.anchorMax = Vector2.one;
        tituloRT.offsetMin = new Vector2(20f, 0f);
        tituloRT.offsetMax = new Vector2(-20f, 0f);

        // Lista de controles
        GameObject controlesGO = new GameObject("Controles");
        controlesGO.transform.SetParent(tarjetaGO.transform, false);
        Text controles = controlesGO.AddComponent<Text>();
        controles.font = fuente;
        controles.fontSize = 20;
        controles.color = new Color(0.2f, 0.2f, 0.2f);
        controles.alignment = TextAnchor.UpperLeft;
        controles.lineSpacing = 1.4f;
        controles.text =
            "WASD — Moverse\n" +
            "Shift — Correr\n" +
            "Q — Cambiar entre cámara 1ra y 3ra persona\n" +
            "E — Agarrar / Soltar un instrumento\n" +
            "F — Verter el reactivo en el vaso de mezcla\n" +
            "T — Ver información de la Tabla Periódica\n" +
            "H — Volver a ver estas instrucciones\n" +
            "Esc — Cerrar cualquier panel abierto";
        RectTransform controlesRT = controlesGO.GetComponent<RectTransform>();
        controlesRT.anchorMin = controlesRT.anchorMax = new Vector2(0.5f, 1f);
        controlesRT.pivot = new Vector2(0.5f, 1f);
        controlesRT.anchoredPosition = new Vector2(0f, -110f);
        controlesRT.sizeDelta = new Vector2(600f, 340f);

        // Boton Entendido
        GameObject cerrarGO = new GameObject("BotonEntendido");
        cerrarGO.transform.SetParent(tarjetaGO.transform, false);
        Image cerrarImg = cerrarGO.AddComponent<Image>();
        cerrarImg.color = new Color(0.2f, 0.55f, 0.3f);
        Button cerrarBtn = cerrarGO.AddComponent<Button>();
        cerrarBtn.onClick.AddListener(Cerrar);
        RectTransform cerrarRT = cerrarGO.GetComponent<RectTransform>();
        cerrarRT.anchorMin = cerrarRT.anchorMax = new Vector2(0.5f, 0f);
        cerrarRT.anchoredPosition = new Vector2(0f, 30f);
        cerrarRT.sizeDelta = new Vector2(200f, 50f);

        GameObject cerrarTextoGO = new GameObject("Texto");
        cerrarTextoGO.transform.SetParent(cerrarGO.transform, false);
        Text cerrarTexto = cerrarTextoGO.AddComponent<Text>();
        cerrarTexto.font = fuente;
        cerrarTexto.fontSize = 20;
        cerrarTexto.fontStyle = FontStyle.Bold;
        cerrarTexto.color = Color.white;
        cerrarTexto.alignment = TextAnchor.MiddleCenter;
        cerrarTexto.text = "Entendido";
        RectTransform cerrarTextoRT = cerrarTextoGO.GetComponent<RectTransform>();
        cerrarTextoRT.anchorMin = Vector2.zero;
        cerrarTextoRT.anchorMax = Vector2.one;
        cerrarTextoRT.offsetMin = Vector2.zero;
        cerrarTextoRT.offsetMax = Vector2.zero;

        panelRaiz.SetActive(false);
        panelCreado = true;
    }
}