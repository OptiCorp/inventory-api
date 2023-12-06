using Inventory.Models;
using Inventory.Models.DTOs.ItemDtos;

namespace Inventory.Utilities
{
    public class ItemUtilities : IItemUtilities
    {
        public ItemResponseDto ItemToResponseDto(Item item)
        {
            var itemResponseDto = new ItemResponseDto
            {
                Id = item.Id,
                WpId = item.WpId,
                SerialNumber = item.SerialNumber,
                ProductNumber = item.ProductNumber,
                Type = item.Type,
                Location = item.Location,
                Description = item.Description,
                ParentId = item.ParentId,
                Vendor = item.Vendor,
                AddedById = item.UserId,
                Comment = item.Comment,
                ListId = item.ListId,
                AddedByFirstName = item.User?.FirstName,
                AddedByLastName = item.User?.LastName,
                CreatedDate = item.CreatedDate.HasValue ? item.CreatedDate+"Z": null,
                UpdatedDate = item.UpdatedDate.HasValue ? item.UpdatedDate+"Z": null
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
                    Location = item.Parent.Location,
                    Description = item.Parent.Description,
                    ParentId = item.Parent.ParentId,
                    Vendor = item.Parent.Vendor,
                    AddedById = item.Parent.UserId,
                    Comment = item.Parent.Comment,
                    ListId = item.Parent.ListId,
                    AddedByFirstName = item.Parent.User?.FirstName,
                    AddedByLastName = item.Parent.User?.LastName,
                    CreatedDate = item.CreatedDate.HasValue ? item.CreatedDate+"Z": null,
                    UpdatedDate = item.UpdatedDate.HasValue ? item.UpdatedDate+"Z": null
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
                        Location = child.Location,
                        Description = child.Description,
                        ParentId = child.ParentId,
                        Vendor = child.Vendor,
                        AddedById = child.UserId,
                        Comment = child.Comment,
                        ListId = child.ListId,
                        AddedByFirstName = child.User?.FirstName,
                        AddedByLastName = child.User?.LastName,
                        CreatedDate = item.CreatedDate.HasValue ? item.CreatedDate+"Z": null,
                        UpdatedDate = item.UpdatedDate.HasValue ? item.UpdatedDate+"Z": null
                    });
                }
                itemResponseDto.Children = children;
            }

            return itemResponseDto;
        }
    }
}
