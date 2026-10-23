namespace SatAPI.Utils;

public static class DataStore
{
    // Diccionario para almacenar el RTU con búsquedas rápidas O(1)
    // Llave: NIT, Valor: Nombre del contribuyente
    public static Dictionary<string, string> Rtu = new Dictionary<string, string>();
}
