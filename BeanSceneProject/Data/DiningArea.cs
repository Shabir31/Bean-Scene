using System.ComponentModel.DataAnnotations.Schema;

namespace BeanSceneProject.Data
{
    public class DiningArea
    {
        public int Id { get; set; }
        public string DiningName { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public List<RestaurantTable> RestaurantTables { get; set; } = new List<RestaurantTable>();

    }
}
