using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.PreCheckValidations;

public interface IPreCheckUpdateValidator
{
    Task<ValidationResult> ValidateAsync(PreCheck preCheck);
}