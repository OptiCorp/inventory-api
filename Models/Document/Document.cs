using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models;

public class Document
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    [MaxLength(100)]
    public string? DocumentTypeId { get; set; }

    [MaxLength(100)]
    public string? BlobId { get; set; }

    [MaxLength(100)]
    public string? ContentType { get; set; }

    [MaxLength(100)]
    public string? ItemId { get; set; }

    [MaxLength(100)]
    public string? ItemTemplateId { get; set; }

    public DocumentType? DocumentType { get; set; }
    
    public string? FileName { get; set; }
}