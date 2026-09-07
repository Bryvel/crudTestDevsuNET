using System.ComponentModel.DataAnnotations;

namespace AccountService.Models
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string AccountNumber { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string AccountType { get; set; } = "Ahorros";
        [MaxLength(1000)]
        public decimal Balance { get; set;}
        [Required]
        public bool state { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public List<Movement> Movements { get; set; } = new();
        public Guid ClientId { get; set; }




    }
    public record AccountCreateDto(Guid ClientId, string AccountType, string AccountNumber, decimal InitialBalance,bool state);

    public record AccountResponseDto(
            Guid Id,
            Guid ClientId,
            string ClientName,
            string AccountNumber,
            string AccountType,
            decimal Balance,
            bool state

         )
    {
        public static AccountResponseDto FromEntity(Account account) => new(
                account.Id,
                account.ClientId,
                account.ClientName,
                account.AccountNumber,
                account.AccountType,
                account.Balance,
                account.state
            );
    }
    public record AccountUpdateDto(string AccountType,bool state);


}
