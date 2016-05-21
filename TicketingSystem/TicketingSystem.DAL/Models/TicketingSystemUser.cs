using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Collections;

namespace TicketingSystem.DAL.Models
{
    public class TicketingSystemUser : IdentityUser
    {

        public enum UserTypes { ADMINISTRATOR, USER };
        [Required]
        public UserTypes UserType { get; set; }

        [StringLength(32, ErrorMessage = "'First Name' must not be longer than 32 characters!")]
        public String FirstName { get; set; }

        [StringLength(32, ErrorMessage = "'Last Name' must not be longer than 32 characters!")]
        public String LastName { get; set; }

        public ICollection<Project> AssignedProjects { get; set; }
        public ICollection<Ticket> CreatedTasks { get; set; }
        public ICollection<Ticket> AssignedTasks { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Change> ChangesCommited { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<TicketingSystemUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return StructuralComparisons.StructuralEqualityComparer.Equals(buffer3, buffer4);
        }
    }
}
