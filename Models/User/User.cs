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
        
        [MaxLength(100)]
        public string? UmId { get; set; }
        
        [MaxLength(100)]
        public string? AzureAdUserId { get; set; }

        [MaxLength(100)]
        public string? UserRole { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Username { get; set; }

        [EnumDataType(typeof(UserStatus))]
        public UserStatus Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
