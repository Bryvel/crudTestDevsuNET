using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
    //La clase cliente hereda de la clase persona, por lo que tiene acceso a los atributos como persona
    public class Client : Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }
        public string Password { get; set; } = string.Empty;
        public bool State { get; set; }

    }

    // Dtos para no exponer la entidad del entity framework directamente

    public record ClientCreateDto(string Name, string Gender, int Age, string CI, string? Addres, string? Phone, string Password, bool State);
    public record ClientUpdateDto(string Name, string Gender, string? Addres, string? Phone, string Password, bool State);
    public record ClientResponseDto(Guid Id, string Name, string Gender, int Age, string? Addres, string CI,  string? Phone, int ClientId, bool state)
    {
        // Proyecta una entidad Client hacia el DTO de salida, enmascarando el
        // documento de identidad para no exponerlo completo por API.
        public static ClientResponseDto FromEntity(Client client) => new(
            client.Id,
            client.Name,
            client.Gender,
            client.Age,
            client.Addres,
            MaskCI(client.CI),
            client.Phone,
            client.ClientId,
            client.State
        );

        private static string MaskCI(string CI)
        {
            if (CI.Length <= 4) return CI;
            var visibleStart = CI[..2];
            var visibleEnd = CI[^2..];
            return $"{visibleStart}{new string('*', CI.Length - 4)}{visibleEnd}";
        }

    }
}
