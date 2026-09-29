using System.ComponentModel.DataAnnotations;

namespace SupportWebApp.Model;

public class User
{
    [Required(ErrorMessage = "Navn skal udfyldes")]
    [StringLength(100, ErrorMessage = "Navn må højst være 100 tegn")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Telefonnummer skal udfyldes")]
    [Phone(ErrorMessage = "Ugyldigt telefonnummer")]
    public string Phone { get; set; } = "";

    [Required(ErrorMessage = "E-mail skal udfyldes")]
    [EmailAddress(ErrorMessage = "Ugyldig e-mailadresse")]
    public string Mail { get; set; } = "";
}
