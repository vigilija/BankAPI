using Microsoft.EntityFrameworkCore;

public class CustomerItemDTO
{
    public string Name { get; set; }
    public int PersonalCode { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }

    public CustomerItemDTO(string name, int personalCode, string address, string email)
    {
        Name = name;
        PersonalCode = personalCode;
        Address = address;
        Email = email;
    }
}
