using System.ComponentModel.DataAnnotations;

namespace AccountService.Models;

    public enum MovementType
{
    Deposito = 1,
    Retiro = 2
}

    public class Movement
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
       public DateTime Date { get; set; } = DateTime.UtcNow;
        public MovementType Type { get; set;}
        [Required]
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public Account? Account { get; set; }
        public Guid AccountId { get; set; }

    public record WithdrawalDto(decimal Amount, string? Description);
    public record DepositDto(decimal Amount, string? Description);

    public record MovementResponseDto(
            Guid Id,
            Guid AccountId,
            string Type,
            decimal Amount,
            decimal Balance
        )
    {
        public static MovementResponseDto FromEntity(Movement movement) => new(
            movement.Id,
            movement.AccountId,
            movement.Type.ToString(),
            movement.Amount,
            movement.Balance
        );
    }
    // DTO de salida para reportes, con datos ya "aplanados"
    public record MovementReportItemDto(
        int MovementId,
        Guid AccountId,
        string AccountNumber,
        int ClientId,
        string ClientName,
        string Type,
        decimal Amount,
        decimal BalanceAfter,
        string? Description,
        DateTime Date
    );

    public record MovementReportDto(
        int TotalMovimientos,
        decimal TotalDepositos,
        decimal TotalRetiros,
        List<MovementReportItemDto> Movimientos
    );



}
