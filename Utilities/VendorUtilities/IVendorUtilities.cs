using Inventory.Models;
using Inventory.Models.DTOs.VendorDTOs;

namespace Inventory.Utilities
{
    public interface IVendorUtilities
    {
        public VendorResponseDto VendorToResponseDto(Vendor vendor);
    }
}