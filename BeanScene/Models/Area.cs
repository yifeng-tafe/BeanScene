using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class Area
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
