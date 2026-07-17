using UnityEngine;

// Este ScriptableObject representa UNA sustancia quimica.
// No se pone sobre un GameObject: se crea como un archivo .asset
// desde el menu Assets > Create > Quimica > Reactivo.
[CreateAssetMenu(fileName = "NuevoReactivo", menuName = "Quimica/Reactivo")]
public class Reactivo : ScriptableObject
{
    [Header("Identificacion")]
    public string nombre;
    public string formula;

    [Header("Propiedades")]
    public Color colorLiquido = Color.white;

    public enum EstadoMateria { Liquido, Solido, Gas }
    public EstadoMateria estado = EstadoMateria.Liquido;
}