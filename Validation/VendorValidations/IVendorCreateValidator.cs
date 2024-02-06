using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.VendorValidations;

public interface IVendorCreateValidator : IValidator<VendorCreateDto>;