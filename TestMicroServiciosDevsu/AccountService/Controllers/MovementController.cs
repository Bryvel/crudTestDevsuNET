using AccountService.Models;
using AccountService.Repositories;
using AccountService.Sevices;
using Microsoft.AspNetCore.Mvc;
using static AccountService.Models.Movement;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/movimientos")]
    public class MovementController:ControllerBase
    {
        private readonly IUnitofWork _unitOfWork;


        public MovementController(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET /api/movimientos Permite consultar todos los movimientos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovementResponseDto>>> GetAll()
        {
            var movements = await _unitOfWork.Movements.GetAllAsync();
            return Ok(movements.Select(MovementResponseDto.FromEntity));
        }

        // GET clients/{id} Retorna todos lo movimientos de una cuenta
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<IEnumerable<MovementResponseDto>>> GetById(Guid id)
        {
            var movements = await _unitOfWork.Movements.GetByAccountIdAsync(id);

            return Ok(movements.Select(MovementResponseDto.FromEntity));
        }

    }
}
