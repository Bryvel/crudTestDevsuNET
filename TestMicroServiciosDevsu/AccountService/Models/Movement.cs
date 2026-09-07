using System.ComponentModel.DataAnnotations;

namespace AccountService.Models;


    public class Movement
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
       public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Type { get; set;} = string.Empty;
    [Required]
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public Account? Account { get; set; }
        public Guid AccountId { get; set; }

    public record MovementDto(decimal Amount);

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
            movement.Type,
            movement.Amount,
            movement.Balance
        );
    }
    // DTO de salida para reportes, con datos ya "aplanados"
    public record MovementReportItemDto(
        Guid MovementId,
        Guid AccountId,
        string AccountNumber,
        Guid ClientId,
        string ClientName,
        string AccountType,
        string Type,
        decimal Amount,
        decimal BalanceAfter,
        DateTime Date,
        bool state
    );

    public record MovementReportDto(
        DateTime Date,
        string NameCliente,
        string AccountNumber,
        string TypeAccount,
        int TotalMovimientos,
        decimal TotalDepositos,
        decimal TotalRetiros,
        bool StateAccount,
        List<MovementReportItemDto> Movimientos
    );



}
