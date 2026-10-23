using Microsoft.AspNetCore.Mvc;
using SatAPI.Models;
using SatAPI.Utils;

namespace SatAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProcesarController : ControllerBase
{
    [HttpPost("rtu")]
    public IActionResult ActualizarRtu([FromBody] List<RtuRequest> solicitudes)
    {
        var respuesta = new RtuResponse();

        foreach (var solicitud in solicitudes)
        {
            // Validar que el NIT no venga nulo y cumpla el algoritmo del Módulo 11
            if (solicitud.Nit == null || !ValidadorNit.EsValido(solicitud.Nit))
            {
                respuesta.NitsInvalidos++;
                continue;
            }

            // Si es válido, verificamos su existencia en el RTU
            if (DataStore.Rtu.ContainsKey(solicitud.Nit))
            {
                DataStore.Rtu[solicitud.Nit] = solicitud.Nombre ?? "";
                respuesta.ContribuyentesActualizados++;
            }
            else
            {
                DataStore.Rtu.Add(solicitud.Nit, solicitud.Nombre ?? "");
                respuesta.ContribuyentesNuevos++;
            }
        }

        return Ok(respuesta);
    }

    [HttpPost("autorizacion")]
    public IActionResult ProcesarAutorizaciones([FromBody] List<AutorizacionRequest> solicitudes)
    {
        // Lógica de Regex para la fecha, verificación de NITs, cálculos matemáticos y correlativos
        return Ok(new AutorizacionResponse());
    }
}

