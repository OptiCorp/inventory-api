using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.ListValidations;

public interface IListCreateValidator : IValidator<List>
{
}