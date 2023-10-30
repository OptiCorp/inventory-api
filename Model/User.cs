using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
    public class User
    {
        public string Id { get; set; }
        public string? Role { get; set; }
        public string? Name { get; set; }
    }
}
