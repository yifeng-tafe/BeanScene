using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class Category
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category")]
        public string Name { get; set; }
    }
}
