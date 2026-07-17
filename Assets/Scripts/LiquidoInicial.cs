using UnityEngine;

// Se pone sobre el GameObject F_Liquid_0X de cada frasco con reactivo fijo
// (Instrumento_01, 02, 03, 05). Al iniciar, busca el reactivo que contiene
// el frasco padre (via ContenedorQuimico) y tiñe el liquido con su color,
// de forma fija -- a diferencia de PuntoMezcla, este no cambia dinamicamente.
public class LiquidoInicial : MonoBehaviour
{
    [Header("Referencias")]
    public Renderer liquidoRenderer; // el Mesh Renderer de este mismo F_Liquid_0X

    [Header("Configuracion visual")]
    [Range(0f, 1f)]
    public float alphaLiquido = 0.85f; // opacidad del liquido ya servido

    void Start()
    {
        if (liquidoRenderer == null)
        {
            liquidoRenderer = GetComponent<Renderer>();
        }

        ContenedorQuimico contenedor = GetComponentInParent<ContenedorQuimico>();

        if (contenedor == null || contenedor.reactivoQueContiene == null)
        {
            Debug.LogWarning($"LiquidoInicial ({gameObject.name}): no se encontro un ContenedorQuimico " +
                              "con reactivo asignado en el padre. El liquido quedara sin colorear.");
            return;
        }

        Color colorReactivo = contenedor.reactivoQueContiene.colorLiquido;
        SetColorLiquido(new Color(colorReactivo.r, colorReactivo.g, colorReactivo.b, alphaLiquido));
    }

    void SetColorLiquido(Color color)
    {
        if (liquidoRenderer == null) return;

        Material mat = liquidoRenderer.material;
        string[] propiedadesPosibles = { "_Color1", "_Color2", "_Color3", "_Color4", "_Color" };

        foreach (string propiedad in propiedadesPosibles)
        {
            if (mat.HasProperty(propiedad))
            {
                mat.SetColor(propiedad, color);
            }
        }
    }
}