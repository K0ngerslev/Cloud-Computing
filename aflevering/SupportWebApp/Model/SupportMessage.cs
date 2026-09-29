using System.ComponentModel.DataAnnotations;


namespace SupportWebApp.Model;

// [ValidatableType] gør, at felterne i den indlejrede User-klasse også bliver valideret.
#pragma warning disable ASP0029
[Microsoft.Extensions.Validation.ValidatableType]
#pragma warning restore ASP0029
public class SupportMessage
{
    [Newtonsoft.Json.JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public User user { get; set; } = new User();

    [Required(ErrorMessage = "Beskrivelse skal udfyldes")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Beskrivelsen skal være mellem 10 og 1000 tegn")]
    public string Description { get; set; } = "";

    // Partition key i CosmosDB er /category
    [Newtonsoft.Json.JsonProperty("category")]
    [Required(ErrorMessage = "Vælg en kategori")]
    public string Category { get; set; } = "";

    public DateTime Date { get; set; }
}
