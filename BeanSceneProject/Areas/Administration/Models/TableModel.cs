using BeanSceneProject.Data;

namespace BeanSceneProject.Areas.Administration.Models
{
    public class TableModel
    {
        public int Id { get; set; }
        public string SeatName { get; set; }

        public int DiningAreaId { get; set; }
        public DiningArea DiningAreas { get; set; }
        public List<ReserveTable> ReserveTables { get; set; } = new List<ReserveTable>();
    }
}
