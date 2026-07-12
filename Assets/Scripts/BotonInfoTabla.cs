using UnityEngine;

// Este script va en el objeto del BOTON (el hijo del Canvas con el componente Button).
// Por ahora solo confirma que el clic funciona con un Debug.Log.
// En el Paso 4 lo conectamos para que abra el panel grid de verdad.
public class BotonInfoTabla : MonoBehaviour
{
    // Este metodo se conecta desde el Inspector, en el evento OnClick() del Button.
    public void AbrirInfoTablaPeriodica()
    {
        Debug.Log("Boton 'Informacion: Tabla Periodica' presionado correctamente.");

        // TODO (Paso 4): aqui vamos a activar el panel grid con los 118 elementos.
    }
}