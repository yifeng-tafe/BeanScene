using BeanScene.Models;

namespace BeanScene.ViewModels
{
    public class FoodViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public Food Food { get; set; }
    }
}
