
namespace Inventory.Models.DTO
{
    public class ItemTemplateCreateDto
    {
        public string? Name { get; set; }
        
        public string? ProductNumber { get; set; }
        
        public string? Type { get; set; }
        
        public string? CategoryId { get; set; }
        
        public string? Revision { get; set; }
        
        public string? CreatedById { get; set; }
        
        public string? Description { get; set; }
    }
}