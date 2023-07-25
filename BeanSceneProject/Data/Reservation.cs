using System.ComponentModel.DataAnnotations.Schema;

namespace BeanSceneProject.Data
{
    public class Reservation
    {
        public int Id { get; set; }
        public string ReservationName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NoOfGuests { get; set; }
        public string? Comments { get; set; }

        public int SittingId { get; set; }
        public Sitting Sitting { get; set; }

        public int ReservationStatusId { get; set; }
        public ReservationStatus ReservationStatus { get; set; }

        public int ReservationTypeId { get; set; }
        public ReservationType ReservationType { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
        public List<ReserveTable> ReserveTables { get; set; } = new List<ReserveTable>();


    }
}
