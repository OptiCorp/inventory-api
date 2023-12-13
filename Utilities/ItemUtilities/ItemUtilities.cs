using Inventory.Models;
using Inventory.Models.DTOs.ItemDtos;

namespace Inventory.Utilities
{
    public class ItemUtilities : IItemUtilities
    {
        private readonly IUserUtilities _userUtilities;
        
        public ItemUtilities(IUserUtilities userUtilities)
        {
            _userUtilities = userUtilities;
        }
        public ItemResponseDto ItemToResponseDto(Item item)
        {
            var itemResponseDto = new ItemResponseDto
            {
                Id = item.Id,
                WpId = item.WpId,
                SerialNumber = item.SerialNumber,
                ProductNumber = item.ProductNumber,
                Type = item.Type,
                CategoryId = item.CategoryId,
                LocationId = item.LocationId,
                VendorId = item.VendorId,
                Description = item.Description,
                ParentId = item.ParentId,
                AddedById = item.UserId,
                Comment = item.Comment,
                ListId = item.ListId,
                AddedByFirstName = item.User?.FirstName,
                AddedByLastName = item.User?.LastName,
                CreatedDate = item.CreatedDate.HasValue ? item.CreatedDate+"Z": null,
                UpdatedDate = item.UpdatedDate.HasValue ? item.UpdatedDate+"Z": null,
                User = item.User != null ? _userUtilities.UserToDto(item.User) : null,
                Vendor = item.Vendor,
                Category = item.Category,
                Location = item.Location,
                LogEntries = item.LogEntries
            };

            if (item.Parent != null)
            {
                itemResponseDto.Parent = new ItemResponseDto
                {
                    Id = item.Parent.Id,
                    WpId = item.Parent.WpId,
                    SerialNumber = item.Parent.SerialNumber,
                    ProductNumber = item.Parent.ProductNumber,
                    Type = item.Parent.Type,
                    CategoryId = item.CategoryId,
                    LocationId = item.LocationId,
                    VendorId = item.VendorId,
                    Description = item.Parent.Description,
                    ParentId = item.Parent.ParentId,
                    AddedById = item.Parent.UserId,
                    Comment = item.Parent.Comment,
                    ListId = item.Parent.ListId,
                    AddedByFirstName = item.Parent.User?.FirstName,
                    AddedByLastName = item.Parent.User?.LastName,
                    CreatedDate = item.CreatedDate.HasValue ? item.CreatedDate+"Z": null,
                    UpdatedDate = item.UpdatedDate.HasValue ? item.UpdatedDate+"Z": null,
                    User = item.User != null ? _userUtilities.UserToDto(item.User) : null,
                    Vendor = item.Vendor,
                    Category = item.Category,
                    Location = item.Location,
                    LogEntries = item.LogEntries
                };
            }
            if (item.Children != null)
            {
                var children = new List<ItemResponseDto>();
                foreach (var child in item.Children)
                {
                    children.Add(new ItemResponseDto
                    {
                        Id = child.Id,
                        WpId = child.WpId,
                        SerialNumber = child.SerialNumber,
                        ProductNumber = child.ProductNumber,
                        Type = child.Type,
                        CategoryId = item.CategoryId,
                        LocationId = item.LocationId,
                        VendorId = item.VendorId,
                        Description = child.Description,
                        ParentId = child.ParentId,
                        AddedById = child.UserId,
                        Comment = child.Comment,
                        ListId = child.ListId,
                        AddedByFirstName = child.User?.FirstName,
                        AddedByLastName = child.User?.LastName,
                        CreatedDate = item.CreatedDate.HasValue ? item.CreatedDate+"Z": null,
                        UpdatedDate = item.UpdatedDate.HasValue ? item.UpdatedDate+"Z": null,
                        User = item.User != null ? _userUtilities.UserToDto(item.User) : null,
                        Vendor = item.Vendor,
                        Category = item.Category,
                        Location = item.Location,
                        LogEntries = item.LogEntries
                    });
                }
                itemResponseDto.Children = children;
            }

            return itemResponseDto;
        }
    }
}
