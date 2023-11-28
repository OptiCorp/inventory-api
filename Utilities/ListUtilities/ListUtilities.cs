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
                CreatedDate = list.CreatedDate,
                UpdatedDate = list.UpdatedDate
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
                        Location = item.Location,
                        Description = item.Description,
                        ParentId = item.ParentId,
                        Vendor = item.Vendor,
                        AddedById = item.UserId,
                        Comment = item.Comment,
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate
                    });
                }
                listReponseDto.Items = items;
            }
            return listReponseDto;
        }
    }
}
