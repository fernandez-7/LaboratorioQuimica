using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelMenu;
    public GameObject panelCreditos;

    [Header("Animación")]
    public float tiempoFade = 1f;
    public CanvasGroup canvasGroup;

    private void Start()
    {
        // Aseguramos que el menú esté visible al iniciar
        if (panelMenu != null) panelMenu.SetActive(true);
        if (panelCreditos != null) panelCreditos.SetActive(false);
        if (canvasGroup != null) canvasGroup.alpha = 0f;

        StartCoroutine(FadeIn());
    }

    // ─── Botón JUGAR ───────────────────────────────
    public void BotonJugar()
    {
        StartCoroutine(CargarEscena("Laboratorio_Principal"));
    }

    // ─── Botón CRÉDITOS ────────────────────────────
    public void BotonCreditos()
    {
        if (panelMenu != null) panelMenu.SetActive(false);
        if (panelCreditos != null) panelCreditos.SetActive(true);
    }

    // ─── Botón VOLVER (desde créditos) ─────────────
    public void BotonVolver()
    {
        if (panelMenu != null) panelMenu.SetActive(true);
        if (panelCreditos != null) panelCreditos.SetActive(false);
    }

    // ─── Botón SALIR ───────────────────────────────
    public void BotonSalir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // ─── Transición con Fade ───────────────────────
    private IEnumerator CargarEscena(string nombreEscena)
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(nombreEscena);
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < tiempoFade)
        {
            t += Time.deltaTime;
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Clamp01(t / tiempoFade);
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float t = tiempoFade;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Clamp01(t / tiempoFade);
            yield return null;
        }
    }
}