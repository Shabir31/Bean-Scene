using Microsoft.AspNetCore.Identity;

namespace BeanSceneProject.Data
{
    public class Member
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //TODO: identity only if member, add code to make it null
        public string? IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

       
    }
}
