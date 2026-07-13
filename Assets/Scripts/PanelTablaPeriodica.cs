using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Este script crea POR CODIGO (igual que Canvas_HUD en SpawnManager.cs)
// el panel de pantalla completa con los 118 elementos en formato de grilla,
// usando xpos/ypos del JSON para ubicar cada celda en su posicion real
// de la tabla periodica (con el hueco de Lantanidos/Actinidos abajo).
//
// Se pone en un GameObject nuevo y vacio, por ejemplo "PanelTablaManager".
public class PanelTablaPeriodica : MonoBehaviour
{
    public static PanelTablaPeriodica Instancia { get; private set; }

    private GameObject panelRaiz;
    private GameObject panelDetalle;
    private Text detalleTextoSimbolo;
    private Text detalleTextoInfo;
    private Text detalleTextoResumen;
    private bool panelCreado = false;
    private bool panelAbierto = false;

    private const float TAMANO_CELDA = 46f;
    private const float ESPACIADO = 3f;
    private const float HUECO_LANTANIDOS = 22f;

    void Awake()
    {
        Instancia = this;
    }

    void Update()
    {
        if (panelAbierto && Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelDetalle != null && panelDetalle.activeSelf)
            {
                CerrarDetalle();
            }
            else
            {
                CerrarPanel();
            }
        }
    }

    public void AbrirPanel()
    {
        if (!panelCreado)
        {
            CrearPanelCompleto();
        }

        panelRaiz.SetActive(true);
        panelAbierto = true;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CerrarPanel()
    {
        if (panelDetalle != null)
        {
            panelDetalle.SetActive(false);
        }

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

    void CrearPanelCompleto()
    {
        Font fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // --- Canvas propio, pantalla completa, por encima de todo ---
        panelRaiz = new GameObject("Canvas_PanelTabla");
        Canvas canvas = panelRaiz.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 50; // por encima del Canvas_HUD (que usa 10)
        CanvasScaler scaler = panelRaiz.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        panelRaiz.AddComponent<GraphicRaycaster>();

        // --- Fondo oscuro semi-transparente, cubre toda la pantalla ---
        GameObject fondoGO = new GameObject("Fondo");
        fondoGO.transform.SetParent(panelRaiz.transform, false);
        Image fondoImg = fondoGO.AddComponent<Image>();
        fondoImg.color = new Color(0f, 0f, 0f, 0.9f);
        RectTransform fondoRT = fondoGO.GetComponent<RectTransform>();
        fondoRT.anchorMin = Vector2.zero;
        fondoRT.anchorMax = Vector2.one;
        fondoRT.offsetMin = Vector2.zero;
        fondoRT.offsetMax = Vector2.zero;

        // --- Titulo ---
        GameObject tituloGO = new GameObject("Titulo");
        tituloGO.transform.SetParent(panelRaiz.transform, false);
        Text titulo = tituloGO.AddComponent<Text>();
        titulo.font = fuente;
        titulo.fontSize = 32;
        titulo.fontStyle = FontStyle.Bold;
        titulo.color = Color.white;
        titulo.alignment = TextAnchor.MiddleCenter;
        titulo.text = "Tabla Periódica de los Elementos";
        RectTransform tituloRT = tituloGO.GetComponent<RectTransform>();
        tituloRT.anchorMin = tituloRT.anchorMax = new Vector2(0.5f, 1f);
        tituloRT.anchoredPosition = new Vector2(0f, -40f);
        tituloRT.sizeDelta = new Vector2(900f, 50f);

        // --- Boton Cerrar (esquina superior derecha) ---
        GameObject cerrarGO = new GameObject("BotonCerrar");
        cerrarGO.transform.SetParent(panelRaiz.transform, false);
        Image cerrarImg = cerrarGO.AddComponent<Image>();
        cerrarImg.color = new Color(0.8f, 0.2f, 0.2f, 1f);
        Button cerrarBtn = cerrarGO.AddComponent<Button>();
        cerrarBtn.onClick.AddListener(CerrarPanel);
        RectTransform cerrarRT = cerrarGO.GetComponent<RectTransform>();
        cerrarRT.anchorMin = cerrarRT.anchorMax = new Vector2(1f, 1f);
        cerrarRT.anchoredPosition = new Vector2(-50f, -40f);
        cerrarRT.sizeDelta = new Vector2(60f, 60f);

        GameObject cerrarTextoGO = new GameObject("Texto");
        cerrarTextoGO.transform.SetParent(cerrarGO.transform, false);
        Text cerrarTexto = cerrarTextoGO.AddComponent<Text>();
        cerrarTexto.font = fuente;
        cerrarTexto.fontSize = 28;
        cerrarTexto.fontStyle = FontStyle.Bold;
        cerrarTexto.color = Color.white;
        cerrarTexto.alignment = TextAnchor.MiddleCenter;
        cerrarTexto.text = "X";
        RectTransform cerrarTextoRT = cerrarTextoGO.GetComponent<RectTransform>();
        cerrarTextoRT.anchorMin = Vector2.zero;
        cerrarTextoRT.anchorMax = Vector2.one;
        cerrarTextoRT.offsetMin = Vector2.zero;
        cerrarTextoRT.offsetMax = Vector2.zero;

        // --- Contenedor de la grilla (se posiciona centrado) ---
        GameObject contenedorGO = new GameObject("ContenedorGrilla");
        contenedorGO.transform.SetParent(panelRaiz.transform, false);
        RectTransform contenedorRT = contenedorGO.AddComponent<RectTransform>();
        contenedorRT.anchorMin = contenedorRT.anchorMax = new Vector2(0.5f, 0.5f);
        contenedorRT.anchoredPosition = new Vector2(0f, -10f);
        contenedorRT.sizeDelta = new Vector2(18 * (TAMANO_CELDA + ESPACIADO), 9 * (TAMANO_CELDA + ESPACIADO) + HUECO_LANTANIDOS);

        // --- Instanciar las 118 celdas usando xpos/ypos del JSON ---
        if (TablaPeriodicaData.Elementos != null)
        {
            foreach (ElementoQuimico elemento in TablaPeriodicaData.Elementos)
            {
                CrearCelda(contenedorGO.transform, elemento, fuente);
            }
        }
        else
        {
            Debug.LogWarning("PanelTablaPeriodica: TablaPeriodicaData.Elementos esta vacio. " +
                              "Verifica que TablaPeriodicaManager este en la escena y cargue antes que este panel.");
        }

        panelRaiz.SetActive(false);
        panelCreado = true;

        CrearPanelDetalle(fuente);
    }

    void CrearPanelDetalle(Font fuente)
    {
        // Este panel se crea DESPUES de la grilla, como ultimo hijo del mismo Canvas,
        // por lo que Unity lo dibuja encima de todo lo anterior automaticamente.
        panelDetalle = new GameObject("PanelDetalle");
        panelDetalle.transform.SetParent(panelRaiz.transform, false);

        // Fondo oscuro que cubre toda la pantalla, oscureciendo la grilla de fondo
        Image overlay = panelDetalle.AddComponent<Image>();
        overlay.color = new Color(0f, 0f, 0f, 0.75f);
        RectTransform overlayRT = panelDetalle.GetComponent<RectTransform>();
        overlayRT.anchorMin = Vector2.zero;
        overlayRT.anchorMax = Vector2.one;
        overlayRT.offsetMin = Vector2.zero;
        overlayRT.offsetMax = Vector2.zero;

        // La tarjeta blanca central
        GameObject tarjetaGO = new GameObject("Tarjeta");
        tarjetaGO.transform.SetParent(panelDetalle.transform, false);
        Image tarjetaImg = tarjetaGO.AddComponent<Image>();
        tarjetaImg.color = new Color(0.95f, 0.95f, 0.95f, 1f);
        RectTransform tarjetaRT = tarjetaGO.GetComponent<RectTransform>();
        tarjetaRT.anchorMin = tarjetaRT.anchorMax = new Vector2(0.5f, 0.5f);
        tarjetaRT.sizeDelta = new Vector2(600f, 520f);
        tarjetaRT.anchoredPosition = Vector2.zero;

        // Encabezado: simbolo grande + nombre y numero al costado, en una sola linea
        GameObject encabezadoGO = new GameObject("Encabezado");
        encabezadoGO.transform.SetParent(tarjetaGO.transform, false);
        Text encabezado = encabezadoGO.AddComponent<Text>();
        encabezado.font = fuente;
        encabezado.fontSize = 30;
        encabezado.fontStyle = FontStyle.Bold;
        encabezado.color = new Color(0.15f, 0.15f, 0.15f);
        encabezado.alignment = TextAnchor.MiddleCenter;
        RectTransform encabezadoRT = encabezadoGO.GetComponent<RectTransform>();
        encabezadoRT.anchorMin = encabezadoRT.anchorMax = new Vector2(0.5f, 1f);
        encabezadoRT.anchoredPosition = new Vector2(0f, -60f);
        encabezadoRT.sizeDelta = new Vector2(560f, 60f);
        detalleTextoSimbolo = encabezado; // reutilizamos la misma referencia

        // Datos: numero atomico, masa, categoria, fase, descubridor
        GameObject infoGO = new GameObject("Info");
        infoGO.transform.SetParent(tarjetaGO.transform, false);
        detalleTextoInfo = infoGO.AddComponent<Text>();
        detalleTextoInfo.font = fuente;
        detalleTextoInfo.fontSize = 18;
        detalleTextoInfo.color = new Color(0.25f, 0.25f, 0.25f);
        detalleTextoInfo.alignment = TextAnchor.UpperLeft;
        detalleTextoInfo.lineSpacing = 1.3f;
        RectTransform infoRT = infoGO.GetComponent<RectTransform>();
        infoRT.anchorMin = infoRT.anchorMax = new Vector2(0.5f, 1f);
        infoRT.anchoredPosition = new Vector2(0f, -140f);
        infoRT.sizeDelta = new Vector2(540f, 130f);

        // Resumen / descripcion en español
        GameObject resumenGO = new GameObject("Resumen");
        resumenGO.transform.SetParent(tarjetaGO.transform, false);
        detalleTextoResumen = resumenGO.AddComponent<Text>();
        detalleTextoResumen.font = fuente;
        detalleTextoResumen.fontSize = 16;
        detalleTextoResumen.color = new Color(0.3f, 0.3f, 0.3f);
        detalleTextoResumen.alignment = TextAnchor.UpperLeft;
        detalleTextoResumen.lineSpacing = 1.2f;
        detalleTextoResumen.horizontalOverflow = HorizontalWrapMode.Wrap;
        detalleTextoResumen.verticalOverflow = VerticalWrapMode.Overflow;
        RectTransform resumenRT = resumenGO.GetComponent<RectTransform>();
        resumenRT.anchorMin = resumenRT.anchorMax = new Vector2(0.5f, 1f);
        resumenRT.anchoredPosition = new Vector2(0f, -300f);
        resumenRT.sizeDelta = new Vector2(540f, 130f);

        // Boton Volver (regresa a la grilla, sin cerrar el panel principal)
        GameObject volverGO = new GameObject("BotonVolver");
        volverGO.transform.SetParent(tarjetaGO.transform, false);
        Image volverImg = volverGO.AddComponent<Image>();
        volverImg.color = new Color(0.2f, 0.4f, 0.75f);
        Button volverBtn = volverGO.AddComponent<Button>();
        volverBtn.onClick.AddListener(CerrarDetalle);
        RectTransform volverRT = volverGO.GetComponent<RectTransform>();
        volverRT.anchorMin = volverRT.anchorMax = new Vector2(0.5f, 0f);
        volverRT.anchoredPosition = new Vector2(0f, 30f);
        volverRT.sizeDelta = new Vector2(180f, 45f);

        GameObject volverTextoGO = new GameObject("Texto");
        volverTextoGO.transform.SetParent(volverGO.transform, false);
        Text volverTexto = volverTextoGO.AddComponent<Text>();
        volverTexto.font = fuente;
        volverTexto.fontSize = 18;
        volverTexto.fontStyle = FontStyle.Bold;
        volverTexto.color = Color.white;
        volverTexto.alignment = TextAnchor.MiddleCenter;
        volverTexto.text = "← Volver";
        RectTransform volverTextoRT = volverTextoGO.GetComponent<RectTransform>();
        volverTextoRT.anchorMin = Vector2.zero;
        volverTextoRT.anchorMax = Vector2.one;
        volverTextoRT.offsetMin = Vector2.zero;
        volverTextoRT.offsetMax = Vector2.zero;

        panelDetalle.SetActive(false);
    }

    void MostrarDetalle(ElementoQuimico e)
    {
        detalleTextoSimbolo.text = $"{e.symbol} - {e.name} (#{e.number})";
        detalleTextoInfo.text =
            $"Masa atómica: {e.atomic_mass} u\n" +
            $"Categoría: {e.category}\n" +
            $"Fase: {e.phase}\n" +
            $"Descubierto por: {e.discovered_by}";
        detalleTextoResumen.text = e.summary;

        panelDetalle.SetActive(true);
    }

    void CerrarDetalle()
    {
        panelDetalle.SetActive(false);
    }

    void CrearCelda(Transform padre, ElementoQuimico elemento, Font fuente)
    {
        GameObject celdaGO = new GameObject("Elemento_" + elemento.symbol);
        celdaGO.transform.SetParent(padre, false);

        Image fondo = celdaGO.AddComponent<Image>();
        fondo.color = ObtenerColorPorCategoria(elemento.category);

        Button boton = celdaGO.AddComponent<Button>();
        ElementoQuimico capturado = elemento; // evita el problema de closures en el foreach
        boton.onClick.AddListener(() => AlHacerClicElemento(capturado));

        RectTransform rt = celdaGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);

        float extraGap = (elemento.ypos >= 8) ? HUECO_LANTANIDOS : 0f;
        float posX = (elemento.xpos - 1) * (TAMANO_CELDA + ESPACIADO);
        float posY = -((elemento.ypos - 1) * (TAMANO_CELDA + ESPACIADO) + extraGap);

        rt.anchoredPosition = new Vector2(posX, posY);
        rt.sizeDelta = new Vector2(TAMANO_CELDA, TAMANO_CELDA);

        // Texto: numero atomico chico arriba, simbolo grande abajo
        GameObject textoGO = new GameObject("Texto");
        textoGO.transform.SetParent(celdaGO.transform, false);
        Text texto = textoGO.AddComponent<Text>();
        texto.font = fuente;
        texto.fontSize = 16;
        texto.fontStyle = FontStyle.Bold;
        texto.color = Color.black;
        texto.alignment = TextAnchor.MiddleCenter;
        texto.text = elemento.number + "\n" + elemento.symbol;
        RectTransform textoRT = textoGO.GetComponent<RectTransform>();
        textoRT.anchorMin = Vector2.zero;
        textoRT.anchorMax = Vector2.one;
        textoRT.offsetMin = Vector2.zero;
        textoRT.offsetMax = Vector2.zero;
    }

    void AlHacerClicElemento(ElementoQuimico elemento)
    {
        MostrarDetalle(elemento);
    }

    Color ObtenerColorPorCategoria(string categoria)
    {
        Dictionary<string, Color> colores = new Dictionary<string, Color>()
        {
            { "Metal alcalino", new Color(1f, 0.6f, 0.6f) },
            { "Metal alcalinotérreo", new Color(1f, 0.75f, 0.4f) },
            { "Metal de transición", new Color(1f, 0.85f, 0.55f) },
            { "Metal post-transición", new Color(0.75f, 0.85f, 0.75f) },
            { "Metaloide", new Color(0.6f, 0.85f, 0.6f) },
            { "No metal poliatómico", new Color(0.55f, 0.9f, 0.6f) },
            { "No metal diatómico", new Color(0.55f, 0.85f, 0.95f) },
            { "Gas noble", new Color(0.75f, 0.6f, 0.95f) },
            { "Lantánido", new Color(0.65f, 0.9f, 0.9f) },
            { "Actínido", new Color(0.85f, 0.6f, 0.9f) },
        };

        if (colores.ContainsKey(categoria))
            return colores[categoria];

        // Categorias "Desconocida (...)" u otras no listadas
        return new Color(0.7f, 0.7f, 0.7f);
    }
}