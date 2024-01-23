
namespace Inventory.Models.DTO
{
    public class ItemCreateDto
    {
        public string? WpId { get; set; }

        public string? SerialNumber { get; set; }

        public string? ParentId { get; set; }

        public string? VendorId { get; set; }

        public string? LocationId { get; set; }

        public string? CreatedById { get; set; }

        public string? Comment { get; set; }
    }
}