

using System.ComponentModel.DataAnnotations;

namespace VerifikationsKodProvider.Data.Entities;

public class VerificationRequestEntity
{
    [Key]
    public string Email { get; set; } = null!;

    public string Code { get; set; } = null!;

    public DateTime ExpiryDate { get; set; } = DateTime.Now.AddMinutes(5);// Här skriver man tiden som man har för att skicka koden 

}
