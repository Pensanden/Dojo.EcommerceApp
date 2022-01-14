using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
       public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
       {
           if (!userManager.Users.Any())
           {
               var user = new AppUser
               {
                   DisplayName = "Roham",
                   Email = "Roham@test.com",
                   UserName = "Roham@test.com",
                   Address = new Address
                   {
                        FirstName = "Roham",
                        LastName = "Moshrefi",
                        Street = "7th Amir Abad",
                        City = "Tehran",
                        State = "Tehran",
                        PostalCode = "1092838421"
                   }

               };
                   await userManager.CreateAsync(user,"Pa$$w0rd");
           }
       } 
    }
}