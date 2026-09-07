using AccountService.Models;
using AccountService.Repositories;
using Microsoft.AspNetCore.Mvc;
using static AccountService.Models.Movement;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/reportes")]
    public class ReportsController : ControllerBase
    {
        private readonly IUnitofWork _unitOfWork;
    // Clase para formatear la fecha para la busqueda de reportes
        private static DateTime? ToUtc(DateTime? value) =>
         value is null
          ? null
          : value.Value.Kind switch
          {
              DateTimeKind.Utc => value,
              DateTimeKind.Unspecified => DateTime.SpecifyKind(value.Value, DateTimeKind.Utc),
              _ => value.Value.ToUniversalTime()
          };

        public ReportsController(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET /api/reportes/movimientos?clientId=&accountId=&desde=&hasta=
        [HttpGet("movimientos")]
        public async Task<ActionResult<MovementReportDto>> GetMovimientos(
        [FromQuery] Guid? clientId,
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta)
        {
        // Postgres necesita saber el tipo de encode que lleva la fecha y necesita ser transformado
            desde = ToUtc(desde);
            hasta = ToUtc(hasta);

            var movimientosEntidad = await _unitOfWork.Movements.GetFilteredAsync(clientId, desde, hasta);


            var movimientos = movimientosEntidad.Select(m => new MovementReportItemDto(
                m.Id,
                m.AccountId,
                m.Account!.AccountNumber,
                m.Account.ClientId,
                m.Account.ClientName,
                m.Account.AccountType,
                m.Type,
                m.Amount,
                m.Balance,
                m.Date,
                m.Account.state

            )).ToList();
          var reporte = new MovementReportDto(
          Date: DateTime.Now,
          NameCliente: movimientos.First().ClientName,
          AccountNumber: movimientos.First().AccountNumber,
          TypeAccount: movimientos.First().AccountType,
          StateAccount: movimientos.First().state,
          TotalMovimientos: movimientos.Count,
          TotalDepositos: movimientos.Where(m => m.Type == "Deposito").Sum(m => m.Amount),
          TotalRetiros: movimientos.Where(m => m.Type == "Retiro").Sum(m => m.Amount),
          Movimientos: movimientos
      );
            return Ok(reporte);

        }
    }
}
