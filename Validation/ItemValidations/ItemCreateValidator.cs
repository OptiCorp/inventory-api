using FluentValidation;
using Inventory.Models.DTOs.ItemDTOs;

namespace Inventory.Validations.ItemValidations
{
    public class ItemCreateValidator : AbstractValidator<ItemCreateDto>
    {

        public ItemCreateValidator()
        {
            RuleFor(item => item.WpId).NotEmpty().WithMessage("WellPartner Id is required.")
                .NotNull().WithMessage("WellPartner Id cannot be null.")
                .MinimumLength(3).WithMessage("WellPartner Id must be at least 3 characters.")
                .MaximumLength(40).WithMessage("WellPartner Id cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.\\- ]+$").WithMessage("WellPartner Id can only contain letters, numbers, underscores, periods or hyphens.");
            
            RuleFor(item => item.SerialNumber).NotEmpty().WithMessage("Serial number is required.")
                .NotNull().WithMessage("Serial number cannot be null.")
                .MinimumLength(3).WithMessage("Serial number must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Serial number cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.\\- ]+$").WithMessage("Serial number can only contain letters, numbers, underscores, periods or hyphens.");
            
            RuleFor(item => item.ProductNumber).NotEmpty().WithMessage("Product number is required.")
                .NotNull().WithMessage("Product number cannot be null.")
                .MinimumLength(3).WithMessage("Product number must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Product number cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.\\- ]+$").WithMessage("Product number can only contain letters, numbers, underscores, periods or hyphens.");
            
            RuleFor(item => item.Type).Must(type => type?.ToLower() is "unit" or "assembly" or "subassembly" or "part").WithMessage("Item type must be Unit, Assembly, Subassembly or Part");
            
            RuleFor(item => item.CategoryId).NotEmpty().WithMessage("Category Id is required.")
                .NotNull().WithMessage("Category Id cannot be null.")
                .MinimumLength(3).WithMessage("Category Id must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Category Id cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.\\- ]+$").WithMessage("Category Id can only contain letters, numbers, underscores, periods or hyphens.");
            
            RuleFor(item => item.ParentId)
                .MinimumLength(3).WithMessage("Parent Id must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Parent Id cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.\\- ]+$").WithMessage("Parent Id can only contain letters, numbers, underscores, periods or hyphens.");
            
            RuleFor(item => item.VendorId).NotEmpty().WithMessage("Vendor Id is required.")
                .NotNull().WithMessage("Vendor Id cannot be null.")
                .MinimumLength(3).WithMessage("Vendor Id must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Vendor Id cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.\\- ]+$").WithMessage("Vendor Id can only contain letters, numbers, underscores, periods or hyphens.");
            
            RuleFor(item => item.LocationId)
                .MinimumLength(3).WithMessage("Location Id must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Location Id cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.\\- ]+$").WithMessage("Location Id can only contain letters, numbers, underscores, periods or hyphens.");

            
            RuleFor(item => item.AddedById).NotEmpty().WithMessage("AddedById is required.")
                .NotNull().WithMessage("AddedById cannot be null.");
            
            RuleFor(item => item.Description).NotEmpty().WithMessage("Description is required.")
                .NotNull().WithMessage("Description cannot be null.")
                .MinimumLength(3).WithMessage("Description must be at least 3 characters.")
                .MaximumLength(450).WithMessage("Description cannot exceed 450 characters.")
                .Matches("^[a-zA-Z0-9_,.\\- ]+$").WithMessage("Description can only contain letters, numbers, underscores, periods or hyphens.");
            
            RuleFor(item => item.Comment)
                .MaximumLength(200).WithMessage("Comment cannot exceed 200 characters.")
                .Matches("^[a-zA-Z0-9_,.\\- ]+$").WithMessage("Comment can only contain letters, numbers, underscores, periods or hyphens.");
        }
    }
}