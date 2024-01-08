using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.VendorValidations;

public interface IVendorCreateValidator
{
    Task<ValidationResult> ValidateAsync(Vendor vendor);
}