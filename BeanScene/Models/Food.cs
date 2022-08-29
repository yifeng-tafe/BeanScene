using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class Food
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]

        public double Price { get; set; }
        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageURL { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public Category Catagory { get; set; }
    }
}
