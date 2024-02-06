using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.SizeValidations;

public interface ISizeCreateValidator : IValidator<SizeCreateDto>;