using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Menu de pausa: se abre con la tecla M, muestra 3 opciones.
// Pausa el juego y libera el cursor mientras esta abierto.
//
// Se pone en un GameObject nuevo y vacio, por ejemplo "PanelMenuPausaManager".
public class PanelMenuPausa : MonoBehaviour
{
    [Header("Nombres exactos de las escenas (Build Settings)")]
    public string nombreEscenaMenuPrincipal = "MenuPrincipal";
    public string nombreEscenaSeleccionPersonaje = "SeleccionPersonaje";

    private GameObject panelRaiz;
    private bool panelCreado = false;
    private bool panelAbierto = false;

    private GameObject textoRecordatorio;
    private bool textoRecordatorioCreado = false;

    void Update()
    {
        if (!textoRecordatorioCreado)
        {
            IntentarCrearRecordatorio();
        }

        if (!panelAbierto && Input.GetKeyDown(KeyCode.M))
        {
            if (OtroPanelAbierto()) return;
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
        bool instruccionesAbiertas = PanelInstrucciones.Instancia != null && PanelInstrucciones.Instancia.EstaAbierto();
        return tablaAbierta || resultadoAbierto || instruccionesAbiertas;
    }

    public void Abrir()
    {
        if (!panelCreado) CrearPanel();

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

    void IrAMenuPrincipal()
    {
        Time.timeScale = 1f; // importante: restaurar el tiempo antes de cambiar de escena
        SceneManager.LoadScene(nombreEscenaMenuPrincipal);
    }

    void IrASeleccionPersonaje()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaSeleccionPersonaje);
    }

    void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego (esto no hace nada dentro del Editor, solo en el .exe compilado).");
        Application.Quit();
    }

    void IntentarCrearRecordatorio()
    {
        GameObject canvasHUD = GameObject.Find("Canvas_HUD");
        if (canvasHUD == null) return;

        Font fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        textoRecordatorio = new GameObject("Texto_MenuRecordatorio");
        textoRecordatorio.transform.SetParent(canvasHUD.transform, false);

        Text texto = textoRecordatorio.AddComponent<Text>();
        texto.font = fuente;
        texto.fontSize = 18;
        texto.color = new Color(1f, 1f, 1f, 0.85f);
        texto.alignment = TextAnchor.MiddleLeft;
        texto.fontStyle = FontStyle.Bold;
        texto.text = "[M] Menú";

        Outline outline = textoRecordatorio.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(1.2f, -1.2f);

        RectTransform rt = textoRecordatorio.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(25f, -25f);
        rt.sizeDelta = new Vector2(150f, 30f);

        textoRecordatorioCreado = true;
    }

    void CrearPanel()
    {
        Font fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        panelRaiz = new GameObject("Canvas_MenuPausa");
        Canvas canvas = panelRaiz.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 80; // el mas alto de todos
        CanvasScaler scaler = panelRaiz.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        panelRaiz.AddComponent<GraphicRaycaster>();

        GameObject fondoGO = new GameObject("Fondo");
        fondoGO.transform.SetParent(panelRaiz.transform, false);
        Image fondoImg = fondoGO.AddComponent<Image>();
        fondoImg.color = new Color(0f, 0f, 0f, 0.85f);
        RectTransform fondoRT = fondoGO.GetComponent<RectTransform>();
        fondoRT.anchorMin = Vector2.zero;
        fondoRT.anchorMax = Vector2.one;
        fondoRT.offsetMin = Vector2.zero;
        fondoRT.offsetMax = Vector2.zero;

        GameObject tarjetaGO = new GameObject("Tarjeta");
        tarjetaGO.transform.SetParent(panelRaiz.transform, false);
        Image tarjetaImg = tarjetaGO.AddComponent<Image>();
        tarjetaImg.color = new Color(0.95f, 0.95f, 0.95f, 1f);
        RectTransform tarjetaRT = tarjetaGO.GetComponent<RectTransform>();
        tarjetaRT.anchorMin = tarjetaRT.anchorMax = new Vector2(0.5f, 0.5f);
        tarjetaRT.sizeDelta = new Vector2(480f, 420f);
        tarjetaRT.anchoredPosition = Vector2.zero;

        GameObject tituloGO = new GameObject("Titulo");
        tituloGO.transform.SetParent(tarjetaGO.transform, false);
        Text titulo = tituloGO.AddComponent<Text>();
        titulo.font = fuente;
        titulo.fontSize = 26;
        titulo.fontStyle = FontStyle.Bold;
        titulo.color = new Color(0.2f, 0.2f, 0.2f);
        titulo.alignment = TextAnchor.MiddleCenter;
        titulo.text = "Menú";
        RectTransform tituloRT = tituloGO.GetComponent<RectTransform>();
        tituloRT.anchorMin = tituloRT.anchorMax = new Vector2(0.5f, 1f);
        tituloRT.pivot = new Vector2(0.5f, 1f);
        tituloRT.anchoredPosition = new Vector2(0f, -30f);
        tituloRT.sizeDelta = new Vector2(400f, 50f);

        CrearBoton(tarjetaGO.transform, fuente, "Volver al Menú Principal", 90f, new Color(0.2f, 0.4f, 0.75f), IrAMenuPrincipal);
        CrearBoton(tarjetaGO.transform, fuente, "Volver a Selección de Personaje", 155f, new Color(0.2f, 0.4f, 0.75f), IrASeleccionPersonaje);
        CrearBoton(tarjetaGO.transform, fuente, "Salir del Juego", 220f, new Color(0.75f, 0.25f, 0.25f), SalirDelJuego);
        CrearBoton(tarjetaGO.transform, fuente, "Continuar Jugando", 320f, new Color(0.3f, 0.55f, 0.3f), Cerrar);

        panelRaiz.SetActive(false);
        panelCreado = true;
    }

    void CrearBoton(Transform padre, Font fuente, string texto, float offsetYDesdeArriba, Color color, UnityEngine.Events.UnityAction accion)
    {
        GameObject botonGO = new GameObject("Boton_" + texto);
        botonGO.transform.SetParent(padre, false);
        Image img = botonGO.AddComponent<Image>();
        img.color = color;
        Button btn = botonGO.AddComponent<Button>();
        btn.onClick.AddListener(accion);
        RectTransform rt = botonGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.anchoredPosition = new Vector2(0f, -offsetYDesdeArriba);
        rt.sizeDelta = new Vector2(380f, 50f);

        GameObject textoGO = new GameObject("Texto");
        textoGO.transform.SetParent(botonGO.transform, false);
        Text t = textoGO.AddComponent<Text>();
        t.font = fuente;
        t.fontSize = 20;
        t.fontStyle = FontStyle.Bold;
        t.color = Color.white;
        t.alignment = TextAnchor.MiddleCenter;
        t.text = texto;
        RectTransform textoRT = textoGO.GetComponent<RectTransform>();
        textoRT.anchorMin = Vector2.zero;
        textoRT.anchorMax = Vector2.one;
        textoRT.offsetMin = Vector2.zero;
        textoRT.offsetMax = Vector2.zero;
    }
}