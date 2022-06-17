using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data.DataModels;

[Table("config")]
public class ConfigDTO
{
    [Column("key"), Key()]
    public string Key { get; set; }
    
    [Column("value")]
    public string Value { get; set; }
}