using System.ComponentModel.DataAnnotations;

namespace UserService.Models

    //Clase Abstracta Person, como base para entidades que hereden de ella como:
    //empleado,cliente,usuario, etc 
{
    public abstract  class Person
    {

        [Key]
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Gender { get; set; } = string.Empty;
        [MaxLength(5)]
        public int Age { get; set; }
        [MaxLength(100)]
        public string CI { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Addres { get; set; }
        [MaxLength(100)]
        public string? Phone { get; set; }

    }
}
