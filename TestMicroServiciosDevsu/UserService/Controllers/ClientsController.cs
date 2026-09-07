using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Repositories;
namespace UserService.Controllers
{

    [ApiController]
    [Route("api/clientes")]
    public class ClientsController:ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // GET /api/clients?page=1&pageSize=20
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientResponseDto>>> GetAll(int page = 1, int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize is < 1 or > 100) pageSize = 20;

            var clients = await _clientRepository.GetPagedAsync(page, pageSize);
            return Ok(clients.Select(ClientResponseDto.FromEntity));
        }

        // GET clients/{id} Retorna un cliente mediante su id unico en la tabla 
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ClientResponseDto>> GetById(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);

            if (client is null)
                return NotFound(new { message = $"Cliente {id} no encontrado" });

            return Ok(ClientResponseDto.FromEntity(client));
        }

        // POST /api/clients Crea un cliente nuevo, con su estado y su clave
        [HttpPost]
        public async Task<ActionResult<ClientResponseDto>> Create(ClientCreateDto dto)
        {
            var exists = await _clientRepository.ExistsByCI(dto.CI);
            if (exists)
                return Conflict(new { message = $"Ya existe un cliente con documento {dto.CI}" });

            var client = new Client
            {
                Name = dto.Name,
                Gender = dto.Gender,
                Age = dto.Age,
                CI= dto.CI,
                Addres = dto.Addres,
                Phone = dto.Phone,
                Password= dto.Password,
                State = dto.State
            };

            await _clientRepository.AddAsync(client);
            await _clientRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = client.Id }, ClientResponseDto.FromEntity(client));
        }


        // PUT /api/clients/{id} Actualiza un cliente a travez de su id unico, solo en los campos que pueden ser modificables 
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<ClientResponseDto>> Update(Guid id, ClientUpdateDto dto)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client is null)
                return NotFound(new { message = $"Cliente {id} no encontrado" });

            client.Name = dto.Name;
            client.Gender = dto.Gender;
            client.Phone = dto.Addres;
            client.Phone = dto.Phone;
            client.Password = dto.Password;
            client.State = dto.State;

            _clientRepository.Update(client);
            await _clientRepository.SaveChangesAsync();

            return Ok(ClientResponseDto.FromEntity(client));
        }
        // DELETE /api/clients/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client is null)
                return NotFound(new { message = $"Cliente {id} no encontrado" });

            _clientRepository.Remove(client);
            await _clientRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
