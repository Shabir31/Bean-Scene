using System.ComponentModel.DataAnnotations.Schema;

namespace BeanSceneProject.Data
{
    public class Sitting
    {
        public int Id { get; set; }
        public string SittingName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        public bool Active { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public int SittingTypeId { get; set; }
        public SittingType SittingType { get; set; }

        public List<Reservation> Reservations { get; set; } = new List<Reservation>(); 


    }
}
