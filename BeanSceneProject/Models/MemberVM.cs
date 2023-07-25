using BeanSceneProject.Data;
using Microsoft.AspNetCore.Identity;

namespace BeanSceneProject.Models
{
    public class MemberVM
    {
        public Member Member { get; set; }
        public IdentityRole Role { get; set; }

    }
}
