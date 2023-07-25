namespace BeanSceneProject.Data
{
    public class ReservationStatus
    {
        public int Id { get; set; }
        public string ReservationStatusName { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>(); 

    }
}
