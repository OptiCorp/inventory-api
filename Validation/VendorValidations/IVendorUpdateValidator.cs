using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.VendorValidations;

public interface IVendorUpdateValidator : IValidator<Vendor>
{
}