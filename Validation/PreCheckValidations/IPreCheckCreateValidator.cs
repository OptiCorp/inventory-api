using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.PreCheckValidations;

public interface IPreCheckCreateValidator : IValidator<PreCheckCreateDto>
{
}