using BeanSceneProject.Data;

namespace BeanSceneProject.Areas.Administration.Models
{
    public class ReservationModel
    {
        public int Id { get; set; }
        public string ReservationName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NoOfGuests { get; set; }
        public string Comments { get; set; }

        public int SittingId { get; set; }
        public Sitting Sitting { get; set; }

        public int ReservationStatusId { get; set; }
        public int ReservationTypeId { get; set; }
        public int MemberId { get; set; }



        public List<Reservation> Reservations { get; set; }
        public List<DiningArea> DiningAreas { get; set; }
        public List<RestaurantTable> RestaurantTables { get; set; }
    }
}
