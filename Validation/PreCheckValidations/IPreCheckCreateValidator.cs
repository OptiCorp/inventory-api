using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.PreCheckValidations;

public interface IPreCheckCreateValidator
{
    Task<ValidationResult> ValidateAsync(PreCheck preCheck);
}