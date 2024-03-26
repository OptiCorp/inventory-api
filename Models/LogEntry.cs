using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models;

public class LogEntry
{
    [MaxLength(100)]

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    [MaxLength(100)]
    public string? ItemId { get; set; }

    [MaxLength(100)]
    public string? ItemTemplateId { get; set; }

    [MaxLength(100)]
    public string? CreatedById { get; set; }

    [MaxLength(200)]
    public string? Message { get; set; }

    public DateTime? CreatedDate { get; set; }

    public User? CreatedBy { get; set; }
}