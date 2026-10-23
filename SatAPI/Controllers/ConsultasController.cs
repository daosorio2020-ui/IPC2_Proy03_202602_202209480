using Microsoft.AspNetCore.Mvc;
using SatAPI.Utils;

namespace SatAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ConsultasController : ControllerBase
{
    // 1. Consultar Contribuyente (Facturas emitidas o recibidas)[span_2](start_span)[span_2](end_span)
    [HttpGet("contribuyente")]
    public IActionResult ConsultarContribuyente(string nit, string tipo)
    {
        var query = DataStore.HistorialFacturas.AsEnumerable();
       
        if (tipo.ToLower() == "emitidas")
            query = query.Where(f => f.NitEmisor == nit);
        else if (tipo.ToLower() == "recibidas")
            query = query.Where(f => f.NitReceptor == nit);

        var reportePorFecha = query.GroupBy(f => f.Fecha)
            .Select(g => new {
                Fecha = g.Key,
                CantidadFacturas = g.Count(),
                MontoTotal = g.Sum(f => f.Total)
            }).ToList();

        var totalGeneral = query.Sum(f => f.Total);

        return Ok(new { Reporte = reportePorFecha, TotalHistorico = totalGeneral });
    }

    // 2. Resumen de IVA por fecha y NIT[span_3](start_span)[span_3](end_span)
    [HttpGet("resumen-iva")]
    public IActionResult ResumenIva(string fecha)
    {
        var facturas = DataStore.HistorialFacturas.Where(f => f.Fecha == fecha).ToList();
       
        // Obtener todos los NITs involucrados en esa fecha (emisores y receptores) sin duplicados
        var nitsUnicos = facturas.Select(f => f.NitEmisor)
            .Concat(facturas.Select(f => f.NitReceptor))
            .Distinct();
       
        var resumen = nitsUnicos.Select(nit => new {
            Nit = nit,
            Nombre = DataStore.Rtu.ContainsKey(nit!) ? DataStore.Rtu[nit!] : "Desconocido",
            IvaEmitido = facturas.Where(f => f.NitEmisor == nit).Sum(f => f.Iva),
            IvaRecibido = facturas.Where(f => f.NitReceptor == nit).Sum(f => f.Iva)
        }).ToList();

        return Ok(resumen);
    }

    // 3. Resumen por rango de fechas (Totales y sin IVA)[span_4](start_span)[span_4](end_span)
    [HttpGet("resumen-rango")]
    public IActionResult ResumenRango()
    {
        // El frontend se encargará de filtrar el rango, aquí devolvemos todo agrupado por fecha
        var resumen = DataStore.HistorialFacturas.GroupBy(f => f.Fecha)
            .Select(g => new {
                Fecha = g.Key,
                TotalConIva = g.Sum(f => f.Total),
                TotalSinIva = g.Sum(f => f.Valor)
            }).ToList();
           
        return Ok(resumen);
    }
}