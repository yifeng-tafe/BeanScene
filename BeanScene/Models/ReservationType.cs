using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class ReservationType
    {
        [Required]
        public int Id { set; get; }
        [Required]
        public string Name { get; set; }
    }
}
