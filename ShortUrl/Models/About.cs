using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShortUrl.Models;

public class About
{
    [Key, Column]
    public int Id { get; set; }

    [Column]
    public string Text { get; set; }
}
