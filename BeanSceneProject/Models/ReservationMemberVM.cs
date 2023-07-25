using BeanSceneProject.Data;

namespace BeanSceneProject.Models
{
    public class ReservationMemberVM
    {

        public List<Reservation> Reservations { get; set; }
        public Member Member { get; set; }
    }
}
