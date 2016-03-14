using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using IdentityManagerWebApp.Dtos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using IdentityManagerWebApp.Models;
using WebGrease.Css.Extensions;

namespace IdentityManagerWebApp
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store, ApplicationDbContext dbContext)
            : base(store)
        {
            _dbContext = dbContext;
        }

        private readonly ApplicationDbContext _dbContext;

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var db = context.Get<ApplicationDbContext>();

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db), db);
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public async Task AddClaimsAsync(string userId, string type, IEnumerable<string> values)
        {

            var tasks = values.Select(a => AddClaimAsync(userId, new Claim(type, a)));

            await Task.WhenAll(tasks);

        }

        public async Task RemoveClaimsAsync(string userId, string type, IEnumerable<string> values)
        {

            var tasks = values.Select(a => RemoveClaimAsync(userId, new Claim(type, a)));

            await Task.WhenAll(tasks);

        }


        public async Task<IEnumerable<UserHeaderDto>> GetPagedAsync(int page, int pageSize)
        {

            return await _dbContext.Users.AsNoTracking()
                                    .OrderBy(x => x.UserName)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(x => new UserHeaderDto()
                                    {
                                        Id = x.Id,
                                        UserName = x.UserName
                                    })
                                    .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _dbContext.Users.CountAsync();
        }

        public async Task<UserDetailsDto> GetDetailsByIdAsync(string id)
        {
            ApplicationUser user = await FindByIdAsync(id);

            UserDetailsDto dto = new UserDetailsDto();
            dto.Id = user.Id;
            dto.UserName = user.UserName;
            dto.Email = user.Email;

            dto.Roles = await GetRolesAsync(user.Id);
            dto.Claims = (await GetClaimsAsync(user.Id)).Select(a => new ClaimDto() {Type = a.Type, Value = a.Value}).ToList();

            return dto;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
