using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models;

public class Size
{
    [MaxLength(100)]

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    [MaxLength(100)]
    public string? ItemTemplateId { get; set; }

    [MaxLength(100)]
    public string? Property { get; set; }

    public float? Amount { get; set; }

    [MaxLength(100)]
    public string? Unit { get; set; }
}