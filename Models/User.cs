using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public enum UserStatus
    {
        [Display(Name = "Active")]
        Active,
        [Display(Name = "Disabled")]
        Disabled,
        [Display(Name = "Deleted")]
        Deleted,
    }
    public class User
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        public string? AzureAdUserId { get; set; }

        public string? UserRoleId { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? Username { get; set; }

        [EnumDataType(typeof(UserStatus))]
        public UserStatus Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        
        public UserRole? UserRole { get; set; }
    }
}
