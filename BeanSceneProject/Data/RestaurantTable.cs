using System.ComponentModel.DataAnnotations.Schema;

namespace BeanSceneProject.Data
{
    public class RestaurantTable
    {
        public int Id { get; set; }
        public string SeatName { get; set; }

        public int DiningAreaId { get; set; }
        public DiningArea DiningAreas { get; set; }
        public List<ReserveTable> ReserveTables { get; set; } = new List<ReserveTable>();
        
               
    }
}
