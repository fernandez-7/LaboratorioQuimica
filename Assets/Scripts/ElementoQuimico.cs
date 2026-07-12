using System;

// Esta clase representa UN elemento quimico.
// No es un MonoBehaviour: no se pone sobre un GameObject.
// Solo sirve para guardar los datos que vienen del JSON.
[Serializable]
public class ElementoQuimico
{
    public int number;
    public string name;
    public string symbol;
    public float atomic_mass;
    public string category;
    public string phase;
    public string discovered_by;
    public string summary;
    public string color_hex;
    public int xpos;
    public int ypos;
    public string name_en;
}

// JsonUtility no puede leer un JSON que empiece directo con una lista "[...]"
// ni uno cuyo nivel superior sea un objeto simple sin una lista nombrada.
// Por eso el JSON tiene la forma { "elementos": [ ... ] },
// y esta clase envolvente permite deserializarlo correctamente.
[Serializable]
public class ListaElementos
{
    public ElementoQuimico[] elementos;
}