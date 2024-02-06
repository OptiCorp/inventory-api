using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.LocationValidations;

public interface ILocationCreateValidator : IValidator<LocationCreateDto>;