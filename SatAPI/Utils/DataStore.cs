namespace SatAPI.Utils;

public static class DataStore
{
    // Diccionario para almacenar el RTU con búsquedas rápidas O(1)
    // Llave: NIT, Valor: Nombre del contribuyente
    public static Dictionary<string, string> Rtu = new Dictionary<string, string>();

        // Registro de referencias para evitar duplicados en tiempo O(1)
    public static HashSet<string> ReferenciasProcesadas = new HashSet<string>();
   
    // Conteo diario de aprobaciones. Llave: Fecha (yyyyMMdd), Valor: correlativo actual
    public static Dictionary<string, int> CorrelativosDiarios = new Dictionary<string, int>();
}
