using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TicketingSystem.Models;
using TicketingSystem.DAL.Models;

namespace TicketingSystem
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<TicketingSystemUser>
    {
        public ApplicationUserManager(IUserStore<TicketingSystemUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<TicketingSystemUser>(context.Get<TicketingSystemDBContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<TicketingSystemUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            return manager;
        }
    }
}
