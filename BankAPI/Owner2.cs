using System.ComponentModel.DataAnnotations;

namespace BankAPI
{
    public class Owner2
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
