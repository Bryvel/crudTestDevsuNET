using AccountService.Models;
using AccountService.Repositories;
using AccountService.Sevices;
using Microsoft.AspNetCore.Mvc;
using static AccountService.Models.Movement;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class AcccountController:ControllerBase
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IClientServiceClient _clientService;

        public AcccountController(IUnitofWork unitOfWork, IClientServiceClient clientService)
        {
            _unitOfWork = unitOfWork;
            _clientService = clientService;
        }
        // GET /api/accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountResponseDto>>> GetAll()
        {
            var accounts = await _unitOfWork.Accounts.GetAllAsync();
            return Ok(accounts.Select(AccountResponseDto.FromEntity));
        }

        // GET clients/{id} Retorna una Cuenta mediante su id unico en la tabla 
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<AccountResponseDto>> GetById(Guid id)
        {
            var client = await _unitOfWork.Accounts.GetByIdAsync(id);

            if (client is null)
                return NotFound(new { message = $"Cliente {id} no encontrado" });

            return Ok(AccountResponseDto.FromEntity(client));
        }

        // POST /api/accounts Crea una nueva cuenta asociada a un Cliente del Client Service
        [HttpPost]
        public async Task<ActionResult<AccountResponseDto>> Create(AccountCreateDto dto)
        {
            var client = await _clientService.GetClientAsync(dto.ClientId);
            if (client is null)
                return BadRequest(new { message = $"El cliente {dto.ClientId} no existe en Client Service" });

            if (dto.InitialBalance < 0)
                return BadRequest(new { message = "El saldo inicial no puede ser negativo" });

            var account = new Account
            {
                ClientId = client.Id,
                ClientName = client.Name,
                AccountNumber = dto.AccountNumber,
                AccountType = dto.AccountType,
                state=dto.state,
                Balance = dto.InitialBalance
            };

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync(); // necesitamos el account.Id ya generado antes de crear el movimiento

            if (dto.InitialBalance > 0)
            {
                var movement = new Movement
                {
                    AccountId = account.Id,
                    Type = "Deposito",
                    Amount = dto.InitialBalance,
                    Balance= account.Balance,
                };
                await _unitOfWork.Movements.AddAsync(movement);
                await _unitOfWork.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetById), new { id = account.Id }, AccountResponseDto.FromEntity(account));
        }

        // PUT /api/accounts/{id}
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<AccountResponseDto>> Update(Guid id, AccountUpdateDto dto)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(id);
            if (account is null)
                return NotFound(new { message = $"Cuenta {id} no encontrada" });

            account.AccountType = dto.AccountType;
            _unitOfWork.Accounts.Update(account);
            await _unitOfWork.SaveChangesAsync();

            return Ok(AccountResponseDto.FromEntity(account));
        }
        // DELETE /api/accounts/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(id);
            if (account is null)
                return NotFound(new { message = $"Cuenta {id} no encontrada" });

            if (account.Balance != 0)
                return BadRequest(new { message = "No se puede eliminar una cuenta con saldo distinto de 0" });

            _unitOfWork.Accounts.Remove(account);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id:Guid}/movimientos")]
        public async Task<IActionResult> Retirar(Guid id, MovementDto dto)
        {

            var account = await _unitOfWork.Accounts.GetByIdAsync(id);
            string typeMovement = "Deposito";
            if (account is null)
                return NotFound(new { message = $"Cuenta {id} no encontrada" });
            if (dto.Amount < 0)
                typeMovement = "Retiro";
            if (account.Balance+dto.Amount<0)
                return BadRequest(new { message = $"Saldo no disponible"});

            account.Balance += dto.Amount;
            _unitOfWork.Accounts.Update(account);

            var movement = new Movement
            {
                AccountId = account.Id,
                Type = typeMovement,
                Amount = dto.Amount,
                Balance = account.Balance,
            };
            await _unitOfWork.Movements.AddAsync(movement);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                account = AccountResponseDto.FromEntity(account),
                movement = MovementResponseDto.FromEntity(movement)
            });
        }
    }
}
