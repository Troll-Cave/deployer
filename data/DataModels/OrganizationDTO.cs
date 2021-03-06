using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data.DataModels;

[Table("organization")]
public class OrganizationDTO
{
    [Column("id"), Key()]
    public Guid ID { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
}