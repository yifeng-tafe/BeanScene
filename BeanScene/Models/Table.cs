using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class Table
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public int AreaID { get; set; }

        [Required]
        public virtual Area Areas { get; set; }
    }
}
