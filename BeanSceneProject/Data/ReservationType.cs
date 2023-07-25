namespace BeanSceneProject.Data
{
    public class ReservationType
    {
        public int Id { get; set; }
        public string ReservationTypeName { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
   
    }
}
