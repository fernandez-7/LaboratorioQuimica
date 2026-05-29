using UnityEngine;
using UnityEngine.SceneManagement;

public class SeleccionPersonaje : MonoBehaviour
{
    public static string personajeElegido = "";

    public void ElegirNino()
    {
        personajeElegido = "Nino";
        SceneManager.LoadScene("Laboratorio_Principal");
    }

    public void ElegirNina()
    {
        personajeElegido = "Nina";
        SceneManager.LoadScene("Laboratorio_Principal");
    }
}