using System.Text.Json.Serialization;

namespace SatAPI.Models;
public class RtuRequest
{
    [JsonPropertyName("NIT")]
    public string? Nit { get; set; }
   
    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }
}

public class RtuResponse
{
    [JsonPropertyName("CONTRIBUYENTES NUEVOS")]
    public int ContribuyentesNuevos { get; set; }
   
    [JsonPropertyName("CONTRIBUYENTES ACTUALIZADOS")]
    public int ContribuyentesActualizados { get; set; }
   
    [JsonPropertyName("NITS INVALIDOS")]
    public int NitsInvalidos { get; set; }
}




public class AutorizacionRequest
{
    [JsonPropertyName("TIEMPO")]
    public string? Tiempo { get; set; }
   
    [JsonPropertyName("REFERENCIA")]
    public string? Referencia { get; set; }
   
    [JsonPropertyName("NIT EMISOR")]
    public string? NitEmisor { get; set; }
   
    [JsonPropertyName("NIT RECEPTOR")]
    public string? NitReceptor { get; set; }
   
    [JsonPropertyName("VALOR")]
    public decimal Valor { get; set; }
   
    [JsonPropertyName("IVA")]
    public decimal Iva { get; set; }
   
    [JsonPropertyName("TOTAL")]
    public decimal Total { get; set; }
}




public class DetalleAprobacion
{
    [JsonPropertyName("referencia")]
    public string? Referencia { get; set; }
   
    [JsonPropertyName("codigoAprobacion")]
    public long CodigoAprobacion { get; set; }
}

public class ResultadoAutorizaciones
{
    [JsonPropertyName("cantidadAutorizacionesAprobadas")]
    public int CantidadAutorizacionesAprobadas { get; set; }
   
    [JsonPropertyName("cantidadAutorizacionesRechazadasPorNitEmisorInexistente")]
    public int RechazadasNitEmisorInexistente { get; set; }
   
    [JsonPropertyName("cantidadAutorizacionesRechazadasPorNitReceptorInexistente")]
    public int RechazadasNitReceptorInexistente { get; set; }
   
    [JsonPropertyName("detalleAprobaciones")]
    public List<DetalleAprobacion> DetalleAprobaciones { get; set; } = new List<DetalleAprobacion>();
}

public class AutorizacionResponse
{
    [JsonPropertyName("resultadoAutorizaciones")]
    public ResultadoAutorizaciones? Resultado { get; set; }
}


