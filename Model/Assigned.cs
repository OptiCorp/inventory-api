using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class Assigned
    {
        public string Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? Count { get; set; }
        public string? EquipmentId { get; set; }
        public string? MaterialId { get; set; }
    }
}
