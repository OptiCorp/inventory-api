using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models;

public class PreCheck
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    public bool? Check { get; set; }
        
    [MaxLength(1000)]
    public string? Comment { get; set; }
}