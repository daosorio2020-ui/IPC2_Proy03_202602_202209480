using Microsoft.AspNetCore.Mvc;
using SatAPI.Models;
using SatAPI.Utils;
using System.Text.RegularExpressions;


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
    var resultado = new ResultadoAutorizaciones();
    var regexFecha = new Regex(@"\d{2}/\d{2}/\d{4}");

    foreach (var factura in solicitudes)
    {
        var match = regexFecha.Match(factura.Tiempo ?? "");
        if (!match.Success) continue;
       
        string fechaLimpia = match.Value;

        decimal ivaCalculado = Math.Round(factura.Valor * 0.12m, 2);
        decimal totalCalculado = factura.Valor + ivaCalculado;

        bool ivaCorrecto = factura.Iva == ivaCalculado;
        bool totalCorrecto = factura.Total == totalCalculado;

        // Validar existencia en RTU
        bool emisorExiste = DataStore.Rtu.ContainsKey(factura.NitEmisor ?? "");
        bool receptorExiste = DataStore.Rtu.ContainsKey(factura.NitReceptor ?? "");
       
        if (!emisorExiste) resultado.RechazadasNitEmisorInexistente++;
        if (!receptorExiste) resultado.RechazadasNitReceptorInexistente++;

        // Validar si la referencia ya fue procesada anteriormente
        bool referenciaDuplicada = DataStore.ReferenciasProcesadas.Contains(factura.Referencia ?? "");

        // Si hay CUALQUIER error, la factura se rechaza y pasamos a la siguiente
        if (!ivaCorrecto || !totalCorrecto || !emisorExiste || !receptorExiste || referenciaDuplicada)
        {
            continue;
        }

        // --- SI LLEGA AQUÍ, LA FACTURA FUE APROBADA ---
       
        // 1. Guardar la referencia para que no se vuelva a usar
        DataStore.ReferenciasProcesadas.Add(factura.Referencia!);
        DataStore.HistorialFacturas.Add(new FacturaHistorial
        {
        Fecha = fechaLimpia,
        NitEmisor = factura.NitEmisor,
        NitReceptor = factura.NitReceptor,
        Valor = factura.Valor,
        Iva = factura.Iva,
        Total = factura.Total});

        // 2. Generar el correlativo diario[span_2](start_span)[span_2](end_span)
        // Convertimos dd/MM/yyyy a yyyyMMdd para la llave del diccionario
        string[] partesFecha = fechaLimpia.Split('/');
        string fechaFormateada = $"{partesFecha[2]}{partesFecha[1]}{partesFecha[0]}";

        if (!DataStore.CorrelativosDiarios.ContainsKey(fechaFormateada))
        {
            DataStore.CorrelativosDiarios[fechaFormateada] = 0;
        }
       
        DataStore.CorrelativosDiarios[fechaFormateada]++;
        int correlativoActual = DataStore.CorrelativosDiarios[fechaFormateada];

        // 3. Ensamblar el código de aprobación (yyyymmdd########)[span_3](start_span)[span_3](end_span)
        string codigoString = $"{fechaFormateada}{correlativoActual.ToString("00000000")}";
        long codigoAprobacion = long.Parse(codigoString);

        // 4. Agregar a la respuesta de éxito[span_4](start_span)[span_4](end_span)
        resultado.CantidadAutorizacionesAprobadas++;
        resultado.DetalleAprobaciones.Add(new DetalleAprobacion
        {
            Referencia = factura.Referencia,
            CodigoAprobacion = codigoAprobacion
        });
    }

    return Ok(new AutorizacionResponse { Resultado = resultado });
}

    [HttpPost("resetear")]
    public IActionResult ResetearDatos()
    {
        DataStore.Resetear();
        return Ok(new { mensaje = "Sistema reiniciado correctamente." });
    }

}

