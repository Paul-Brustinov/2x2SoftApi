using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace _2x2SoftApi.Mvc.Models.IdentityModels
{

    [Flags]
    public enum UserModifiedFlag
    {
        Default = 0,
        Lockout = 0x0001,
        Password = 0x0002
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IUser<Int64>
    {
        public ApplicationUser(Int64 id) { Id = id; }
        public ApplicationUser() { Id = 0; }

        #region IUser<Int64>
        public Int64 Id { get; set; }
        public string UserName { get; set; }
        #endregion

        public String PersonName { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }

        public String PasswordHash { get; set; }
        public String SecurityStamp { get; set; }
        public DateTimeOffset LockoutEndDateUtc { get; set; }
        public Boolean LockoutEnabled { get; set; }
        public Int32 AccessFailedCount { get; set; }
        public Boolean TwoFactorEnabled { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public Boolean PhoneNumberConfirmed { get; set; }

        public Guid Guid { get; set; }
        UserModifiedFlag _modifiedFlag;

        public void SetModified(UserModifiedFlag flag)
        {
            _modifiedFlag |= flag;
        }

        public void ClearModified(UserModifiedFlag flag)
        {
            _modifiedFlag &= ~flag;
        }

        public Boolean IsLockoutModified => _modifiedFlag.HasFlag(UserModifiedFlag.Lockout);

        public bool IsPasswordModified => _modifiedFlag.HasFlag(UserModifiedFlag.Password);

        public string Locale { get; set; }
        public string Memo { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Int64> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext()
    //        : base("DefaultConnection", throwIfV1Schema: false)
    //    {
    //    }
        
    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }
    //}
}