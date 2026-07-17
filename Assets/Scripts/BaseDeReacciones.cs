using UnityEngine;
using System.Collections.Generic;

// Contiene la lista completa de reacciones validas del juego.
// Se crea UN SOLO asset de este tipo (Assets > Create > Quimica > Base De Reacciones)
// y se le arrastran todas las ReaccionQuimica que existan.
[CreateAssetMenu(fileName = "BaseDeReacciones", menuName = "Quimica/Base De Reacciones")]
public class BaseDeReacciones : ScriptableObject
{
    public List<ReaccionQuimica> reacciones;

    // Busca si existe una receta para estos dos reactivos. Devuelve null si no existe.
    public ReaccionQuimica BuscarReaccion(Reactivo a, Reactivo b)
    {
        if (reacciones == null) return null;

        foreach (ReaccionQuimica receta in reacciones)
        {
            if (receta != null && receta.Coincide(a, b))
                return receta;
        }

        return null;
    }
}