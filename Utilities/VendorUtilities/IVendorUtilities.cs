using Inventory.Models;
using Inventory.Models.DTOs.VendorDtos;

namespace Inventory.Utilities
{
    public interface IVendorUtilities
    {
        public VendorResponseDto VendorToResponseDto(Vendor vendor);
    }
}