using BeanSceneProject.Data;

namespace BeanSceneProject.Areas.Administration.Models
{
    public class ViewCalendarVM
    {
        public int Id { get; set; }
        public string SittingName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        public bool Active { get; set; }

        public int RestaurantId { get; set; }

        public int SittingTypeId { get; set; }
        public List<Restaurant> Restaurants { get; set; }
        public List<SittingType> sittingTypes { get; set; }

    }
   
}
