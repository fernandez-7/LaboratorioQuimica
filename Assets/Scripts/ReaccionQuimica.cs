using UnityEngine;

// Define UNA receta: que pasa cuando se combinan dos Reactivos especificos.
// Se crea como asset desde Assets > Create > Quimica > Reaccion Quimica.
[CreateAssetMenu(fileName = "NuevaReaccion", menuName = "Quimica/Reaccion Quimica")]
public class ReaccionQuimica : ScriptableObject
{
    [Header("Reactivos que la componen (el orden no importa)")]
    public Reactivo reactivoA;
    public Reactivo reactivoB;

    [Header("Resultado")]
    public string nombreReaccion;
    public Color colorResultado = Color.green;
    public bool produceBurbujeo = true;

    [Header("Texto educativo (en español)")]
    [TextArea(3, 6)]
    public string mensajeEducativo;

    // Revisa si esta receta corresponde a los dos reactivos dados,
    // sin importar en que orden se vertieron.
    public bool Coincide(Reactivo a, Reactivo b)
    {
        if (a == null || b == null) return false;

        bool ordenNormal = (reactivoA == a && reactivoB == b);
        bool ordenInvertido = (reactivoA == b && reactivoB == a);

        return ordenNormal || ordenInvertido;
    }
}