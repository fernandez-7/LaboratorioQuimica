using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject prefabNino;
    public GameObject prefabNina;
    public Transform puntoSpawn;

    void Start()
    {
        string personaje = SeleccionPersonaje.personajeElegido;
        GameObject prefabElegido = (personaje == "Nina") ? prefabNina : prefabNino;

        GameObject jugador = Instantiate(prefabElegido, puntoSpawn.position, puntoSpawn.rotation);

        CharacterController cc = jugador.AddComponent<CharacterController>();
        cc.height = 1.8f;
        cc.center = new Vector3(0, 0.9f, 0);
        cc.radius = 0.3f;

        ControladorPersonaje cp = jugador.AddComponent<ControladorPersonaje>();

        // Cámara primera persona
        GameObject camFP = new GameObject("Camara_PrimeraPersona");
        camFP.transform.SetParent(jugador.transform);
        camFP.transform.localPosition = new Vector3(0, 1.6f, 0.1f);
        camFP.transform.localRotation = Quaternion.identity;
        Camera camaraFP = camFP.AddComponent<Camera>();
        camFP.tag = "MainCamera";
        camFP.AddComponent<AudioListener>();

        // Cámara tercera persona
        GameObject camTP = new GameObject("Camara_TerceraPersona");
        camTP.transform.SetParent(jugador.transform);
        camTP.transform.localPosition = new Vector3(0, 2f, -4f);
        camTP.transform.localRotation = Quaternion.Euler(15f, 0f, 0f);
        Camera camaraTP = camTP.AddComponent<Camera>();
        camaraTP.gameObject.SetActive(false);

        cp.camaraPrimeraPersona = camaraFP;
        cp.camaraTerceraPersona = camaraTP;

        // PuntoAgarre hijo de la cámara primera persona
        GameObject puntoAgarre = new GameObject("PuntoAgarre");
        puntoAgarre.transform.SetParent(camFP.transform);
        puntoAgarre.transform.localPosition = new Vector3(0, -0.2f, 1.2f);

        // Canvas HUD
        GameObject canvasGO = new GameObject("Canvas_HUD");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        // Crosshair — línea horizontal
        GameObject lineaH = new GameObject("Crosshair_H");
        lineaH.transform.SetParent(canvasGO.transform, false);
        Image imgH = lineaH.AddComponent<Image>();
        imgH.color = new Color(1f, 1f, 1f, 0.9f);
        RectTransform rtH = lineaH.GetComponent<RectTransform>();
        rtH.anchorMin = rtH.anchorMax = new Vector2(0.5f, 0.5f);
        rtH.sizeDelta = new Vector2(16f, 2f);
        rtH.anchoredPosition = Vector2.zero;

        // Crosshair — línea vertical
        GameObject lineaV = new GameObject("Crosshair_V");
        lineaV.transform.SetParent(canvasGO.transform, false);
        Image imgV = lineaV.AddComponent<Image>();
        imgV.color = new Color(1f, 1f, 1f, 0.9f);
        RectTransform rtV = lineaV.GetComponent<RectTransform>();
        rtV.anchorMin = rtV.anchorMax = new Vector2(0.5f, 0.5f);
        rtV.sizeDelta = new Vector2(2f, 16f);
        rtV.anchoredPosition = Vector2.zero;

        // Texto [E] Agarrar / Soltar
        Font fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        GameObject textoGO = new GameObject("Texto_Interaccion");
        textoGO.transform.SetParent(canvasGO.transform, false);
        Text textoInteraccion = textoGO.AddComponent<Text>();
        textoInteraccion.font = fuente;
        textoInteraccion.fontSize = 22;
        textoInteraccion.color = Color.white;
        textoInteraccion.alignment = TextAnchor.MiddleCenter;
        textoInteraccion.fontStyle = FontStyle.Bold;
        Outline outline = textoGO.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(1.5f, -1.5f);
        RectTransform textoRT = textoGO.GetComponent<RectTransform>();
        textoRT.anchorMin = textoRT.anchorMax = new Vector2(0.5f, 0.5f);
        textoRT.sizeDelta = new Vector2(220f, 40f);
        textoRT.anchoredPosition = new Vector2(0f, -50f);
        textoGO.SetActive(false);

        // InteraccionJugador
        InteraccionJugador interaccion = jugador.AddComponent<InteraccionJugador>();
        interaccion.puntoAgarre = puntoAgarre.transform;
        interaccion.distanciaInteraccion = 2f;
        interaccion.camaraPrimeraPersona = camaraFP;
        interaccion.camaraTerceraPersona = camaraTP;
        interaccion.textoInteraccion = textoInteraccion;
    }
}