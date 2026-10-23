using SatAPI.Models;
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

    public static List<FacturaHistorial> HistorialFacturas = new List<FacturaHistorial>();

    // Método para devolver el sistema a su estado inicial[span_3](start_span)[span_3](end_span)
    public static void Resetear()
    {
        Rtu.Clear();
        ReferenciasProcesadas.Clear();
        CorrelativosDiarios.Clear();
        HistorialFacturas.Clear();
    }

}
