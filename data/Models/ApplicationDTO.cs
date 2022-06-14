using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data.Models;

[Table("application")]
public class ApplicationDTO
{
    [Column("id"), Key()]
    public Guid ID { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
}