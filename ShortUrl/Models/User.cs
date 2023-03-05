using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShortUrl.Models;

public class User
{
    [Key, Column]
    public string Login { get; set; }

    [Required, Column]
    public string Password { get; set; }

    [Required, Column(TypeName = "nvarchar(32)")]
    public UserRole Role { get; set; }

    public List<UrlPair> UrlPairs { get; set; }
}
