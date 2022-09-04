using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class Role
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName(displayName: "Role")]
        public string Name { get; set; }
    }
}
