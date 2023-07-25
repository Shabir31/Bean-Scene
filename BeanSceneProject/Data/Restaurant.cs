namespace BeanSceneProject.Data
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public List<DiningArea> DiningAreas { get; set; } = new List<DiningArea>();
        public List<Sitting> Sittings { get; set; } = new List<Sitting>();
    }
}
