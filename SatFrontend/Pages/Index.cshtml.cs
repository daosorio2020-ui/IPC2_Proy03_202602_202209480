using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SatFrontend.Pages;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
   
    [TempData]
    public string? MensajeResultado { get; set; }

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostCargarRtuAsync(IFormFile? archivoJson)
    {
        return await EnviarArchivoApi(archivoJson, "/Procesar/rtu");
    }

    public async Task<IActionResult> OnPostCargarFacturasAsync(IFormFile? archivoJson)
    {
        return await EnviarArchivoApi(archivoJson, "/Procesar/autorizacion");
    }

    private async Task<IActionResult> EnviarArchivoApi(IFormFile? archivoJson, string endpoint)
    {
        if (archivoJson == null || archivoJson.Length == 0)
        {
            MensajeResultado = "Por favor selecciona un archivo válido.";
            return Page();
        }

        using var stream = new StreamReader(archivoJson.OpenReadStream());
        string contenidoJson = await stream.ReadToEndAsync();

        var client = _httpClientFactory.CreateClient("SatApi");
        var content = new StringContent(contenidoJson, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync(endpoint, content);
       
        if (response.IsSuccessStatusCode)
        {
            var respuestaString = await response.Content.ReadAsStringAsync();
            MensajeResultado = $"Éxito: {respuestaString}";
        }
        else
        {
            MensajeResultado = $"Error de comunicación con la API: {response.StatusCode}";
        }

        return Page();
    }
}