using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.ListValidations;

public interface IListCreateValidator : IValidator<ListCreateDto>
{
}