using BeanScene.Models;

namespace BeanScene.ViewModels
{
    public class TableViewModel
    {
        public IEnumerable<Area>? Areas { get; set; }
        public Table Table { get; set; }
    }
}
