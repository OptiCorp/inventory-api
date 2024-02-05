using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models;

public class ItemTemplate
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
        
    [MaxLength(100)]
    public string? Type { get; set; }
        
    [MaxLength(100)]
    public string? CategoryId { get; set; }
        
    [MaxLength(100)]
    public string? ProductNumber { get; set; }
        
    [MaxLength(100)]
    public string? Revision { get; set; }
        
    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
        
    [MaxLength(100)]
    public string? CreatedById { get; set; }

    public User? CreatedBy { get; set; }

    public Category? Category { get; set; }

    public ICollection<Document>? Documents { get; set; }

    public IEnumerable<Size>? Sizes { get; set; }

    public IEnumerable<LogEntry>? LogEntries { get; set; }
}