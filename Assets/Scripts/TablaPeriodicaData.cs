using UnityEngine;

// Este script carga los 118 elementos UNA sola vez al iniciar el juego
// y los deja guardados en memoria para que cualquier otro script los use,
// sin tener que leer el archivo JSON otra vez.
//
// Se pone en un GameObject vacio en la escena (por ejemplo, dentro de SpawnManager
// o en un GameObject nuevo llamado "TablaPeriodicaManager").
public class TablaPeriodicaData : MonoBehaviour
{
    // Lista publica y estatica: cualquier script puede acceder escribiendo
    // TablaPeriodicaData.Elementos, sin necesitar una referencia directa a este objeto.
    public static ElementoQuimico[] Elementos { get; private set; }

    void Awake()
    {
        CargarDatos();
    }

    void CargarDatos()
    {
        // Resources.Load busca el archivo dentro de cualquier carpeta "Resources"
        // del proyecto. NO se escribe la extension ".json".
        TextAsset archivoJson = Resources.Load<TextAsset>("PeriodicTable");

        if (archivoJson == null)
        {
            Debug.LogError("No se encontro PeriodicTable.json en Assets/Resources/. " +
                            "Verifica que el archivo este en esa carpeta exacta.");
            return;
        }

        // Convierte el texto del JSON en objetos ElementoQuimico
        ListaElementos datos = JsonUtility.FromJson<ListaElementos>(archivoJson.text);

        if (datos == null || datos.elementos == null || datos.elementos.Length == 0)
        {
            Debug.LogError("El JSON se cargo pero no contiene elementos. Revisa el formato.");
            return;
        }

        Elementos = datos.elementos;

        Debug.Log($"Tabla periodica cargada correctamente: {Elementos.Length} elementos.");

        // Prueba: mostrar el primero y el ultimo elemento para confirmar
        // que los datos llegaron bien, en espanol y completos.
        ElementoQuimico primero = Elementos[0];
        ElementoQuimico ultimo = Elementos[Elementos.Length - 1];

        Debug.Log($"Primero -> #{primero.number} {primero.name} ({primero.symbol}) " +
                  $"| Categoria: {primero.category} | Fase: {primero.phase}");

        Debug.Log($"Ultimo -> #{ultimo.number} {ultimo.name} ({ultimo.symbol}) " +
                  $"| Categoria: {ultimo.category} | Fase: {ultimo.phase}");
    }

    // Metodo util que vamos a necesitar mas adelante (Paso 5 y 6):
    // busca un elemento por su numero atomico.
    public static ElementoQuimico ObtenerPorNumero(int numero)
    {
        if (Elementos == null) return null;

        foreach (ElementoQuimico elemento in Elementos)
        {
            if (elemento.number == numero)
                return elemento;
        }
        return null;
    }
}