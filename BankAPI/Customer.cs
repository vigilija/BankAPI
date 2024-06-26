using System.ComponentModel.DataAnnotations;

namespace BankAPI
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int PersonalCode { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
