
namespace Inventory.Models.DTO
{
    public class SizeCreateDto
    {
        public string? ItemTemplateId { get; set; }

        public string? Property { get; set; }

        public float? Amount { get; set; }

        public string? Unit { get; set; }
    }
}