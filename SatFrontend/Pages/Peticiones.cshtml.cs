using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SatFrontend.Pages;

public class PeticionesModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    // Variables para almacenar los resultados y mostrarlos en el HTML
    public string? ResultadoJson { get; set; }
    public string PestañaActiva { get; set; } = "contribuyente";

    public PeticionesModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public void OnGet()
    {
    }

    // 1. Consultar Contribuyente[span_1](start_span)[span_1](end_span)
    public async Task<IActionResult> OnPostConsultarContribuyenteAsync(string nit, string tipo)
    {
        PestañaActiva = "contribuyente";
        var client = _httpClientFactory.CreateClient("SatApi");
       
        var response = await client.GetAsync($"/Consultas/contribuyente?nit={nit}&tipo={tipo}");
        if (response.IsSuccessStatusCode)
        {
            // Formateamos el JSON para que se vea ordenado en pantalla por el momento
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<object>(json);
            ResultadoJson = JsonSerializer.Serialize(obj, options);
        }
        else
        {
            ResultadoJson = "Error o no se encontraron datos.";
        }
        return Page();
    }

    // 2. Resumen de IVA[span_2](start_span)[span_2](end_span)
    public async Task<IActionResult> OnPostResumenIvaAsync(string fecha)
    {
        PestañaActiva = "iva";
        var client = _httpClientFactory.CreateClient("SatApi");
       
        var response = await client.GetAsync($"/Consultas/resumen-iva?fecha={fecha}");
        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<object>(json);
            ResultadoJson = JsonSerializer.Serialize(obj, options);
        }
        return Page();
    }
}
