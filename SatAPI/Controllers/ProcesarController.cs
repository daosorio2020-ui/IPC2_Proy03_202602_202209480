using SatAPI.Models;
using Microsoft.AspNetCore.Mvc;
namespace SatAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProcesarController : ControllerBase
{
    [HttpPost("rtu")]
    public IActionResult ActualizarRtu([FromBody] List<RtuRequest> solicitudes)
    {
        // Lógica de validación del módulo 11 e inserción en el Dictionary de contribuyentes
        return Ok(new RtuResponse());
    }

    [HttpPost("autorizacion")]
    public IActionResult ProcesarAutorizaciones([FromBody] List<AutorizacionRequest> solicitudes)
    {
        // Lógica de Regex para la fecha, verificación de NITs, cálculos matemáticos y correlativos
        return Ok(new AutorizacionResponse());
    }
}
