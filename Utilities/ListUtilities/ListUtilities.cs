using Inventory.Models;
using Inventory.Models.DTOs.ListDTOs;
using Inventory.Models.DTOs.ItemDTOs;

namespace Inventory.Utilities
{
    public class ListUtilities : IListUtilities
    {
        private readonly IUserUtilities _userUtilities;
        private readonly IItemUtilities _itemUtilities;
        
        public ListUtilities(IUserUtilities userUtilities, IItemUtilities itemUtilities)
        {
            _userUtilities = userUtilities;
            _itemUtilities = itemUtilities;
        }
        public ListResponseDto ListToResponseDto(List list)
        {
            var listReponseDto = new ListResponseDto
            {
                Id = list.Id,
                Title = list.Title,
                CreatedById = list.UserId,
                CreatedDate = list.CreatedDate.HasValue ? list.CreatedDate+"Z": null,
                UpdatedDate = list.UpdatedDate.HasValue ? list.UpdatedDate+"Z": null,
                User = list.User != null ? _userUtilities.UserToDto(list.User): null
            };
            
            if (list.Items != null)
            {
                var items = new List<ItemResponseDto>();
                foreach (var item in list.Items)
                {
                    items.Add(_itemUtilities.ItemToResponseDto(item));
                }
                listReponseDto.Items = items;
            }
            return listReponseDto;
        }
    }
}
