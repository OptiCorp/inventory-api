using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.ItemValidations;

public interface IItemCreateValidator : IValidator<ItemCreateDto>;