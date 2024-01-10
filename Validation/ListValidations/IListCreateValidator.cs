using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.ListValidations;

public interface IListCreateValidator : IValidator<List>
{
}