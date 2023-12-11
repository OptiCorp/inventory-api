using Inventory.Models;
using Inventory.Models.DTOs.ListDtos;
using Inventory.Models.DTOs.ItemDtos;

namespace Inventory.Utilities
{
    public class ListUtilities : IListUtilities
    {
        public ListResponseDto ListToResponseDto(List list)
        {
            var listReponseDto = new ListResponseDto
            {
                Id = list.Id,
                Title = list.Title,
                CreatedById = list.UserId,
                CreatedDate = list.CreatedDate.HasValue ? list.CreatedDate+"Z": null,
                UpdatedDate = list.UpdatedDate.HasValue ? list.UpdatedDate+"Z": null
            };
            
            if (list.Items != null)
            {
                var items = new List<ItemResponseDto>();
                foreach (var item in list.Items)
                {
                    items.Add(new ItemResponseDto
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
                        UpdatedDate = item.UpdatedDate.HasValue ? item.UpdatedDate+"Z": null
                    });
                }
                listReponseDto.Items = items;
            }
            return listReponseDto;
        }
    }
}
