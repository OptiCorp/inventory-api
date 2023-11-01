using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public enum Status
    {
        [Display(Name = "In Use")]
        InUse,
        [Display(Name = "Available")]
        Available,
        [Display(Name = "Need Inspection")]
        NeedInspection,
        [Display(Name = "Broken")]
        Broken
    }
    public enum EquipmentType
    {
        [Display(Name = "Machine")]
        Machine,
        [Display(Name = "Tool")]
        Tool
    }
    public class Equipment
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        [EnumDataType(typeof(Status))]
        public Status? Status { get; set; }
        public DateTime? LastInspected { get; set; }
        public DateTime? NextInspected { get; set; }
        [EnumDataType(typeof(EquipmentType))]
        public EquipmentType? EquipmentType { get; set; }
    }
}
