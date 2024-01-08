using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.VendorValidations;

public interface IVendorUpdateValidator
{
    Task<ValidationResult> ValidateAsync(Vendor vendor);
}