using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShortUrl.Models;

public class UrlPair
{
    [Key, Column]
    public string ShortUrl { get; set; }

    [Required, Column]
    public string LongUrl { get; set; }

    [Required, Column, ForeignKey("User")]
    public string CreatedBy { get; set; }

    public User User { get; set; }

    [Required, Column]
    public DateTime CreatedDateTime { get; set; }
}
