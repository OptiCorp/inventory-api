using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public enum Type
    {
        Wood,
        Metal,
        Plastic,
        Other
    }
    public class Material
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public int? StockCount { get; set; }
        [EnumDataType(typeof(Type))]
        public string? Type { get; set; }
    }
}
