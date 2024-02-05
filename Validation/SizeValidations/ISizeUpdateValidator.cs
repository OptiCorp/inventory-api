using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.SizeValidations;

public interface ISizeUpdateValidator : IValidator<Size>;