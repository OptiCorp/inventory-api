using Inventory.Models;
using Inventory.Models.DTOs.VendorDtos;

namespace Inventory.Utilities
{
    public class VendorUtilities : IVendorUtilities
    {
        private readonly IUserUtilities _userUtilities;
        
        public VendorUtilities(IUserUtilities userUtilities)
        {
            _userUtilities = userUtilities;
        }
        public VendorResponseDto VendorToResponseDto(Vendor vendor)
        {
            return new VendorResponseDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                Address = vendor.Address,
                Email = vendor.Email,
                PhoneNumber = vendor.PhoneNumber,
                AddedById = vendor.UserId,
                CreatedDate = vendor.CreatedDate.HasValue ? vendor.CreatedDate+"Z": null,
                UpdatedDate = vendor.UpdatedDate.HasValue ? vendor.UpdatedDate+"Z": null,
                User = vendor.User != null ? _userUtilities.UserToDto(vendor.User) : null,
            };
        }
    }
}