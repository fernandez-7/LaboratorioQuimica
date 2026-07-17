using UnityEngine;

// Se agrega a cada frasco/instrumento que va a funcionar como reactivo.
// Simplemente guarda una referencia al Reactivo (ScriptableObject) que representa.
public class ContenedorQuimico : MonoBehaviour
{
    public Reactivo reactivoQueContiene;
}