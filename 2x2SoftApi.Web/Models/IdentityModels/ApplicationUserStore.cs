using A2v10.Data.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace _2x2SoftApi.Mvc.Models.IdentityModels
{
    public class ApplicationUserStore :
        IDisposable,
        IUserStore<ApplicationUser, Int64>,
        IUserLoginStore<ApplicationUser, Int64>,
        IUserLockoutStore<ApplicationUser, Int64>,
        IUserPasswordStore<ApplicationUser, Int64>,
        IUserTwoFactorStore<ApplicationUser, Int64>,
        IUserEmailStore<ApplicationUser, Int64>,
        IUserPhoneNumberStore<ApplicationUser, Int64>,
        IUserSecurityStampStore<ApplicationUser, Int64>,
        IUserRoleStore<ApplicationUser, Int64>,
        IUserClaimStore<ApplicationUser, Int64>
    {

        class UserCache
        {
            Dictionary<String, ApplicationUser> _mapNames = new Dictionary<String, ApplicationUser>();
            Dictionary<String, ApplicationUser> _mapEmails = new Dictionary<String, ApplicationUser>();
            Dictionary<Int64, ApplicationUser> _mapIds = new Dictionary<Int64, ApplicationUser>();

            public ApplicationUser GetById(Int64 id)
            {
                ApplicationUser user = null;
                if (_mapIds.TryGetValue(id, out user))
                    return user;
                return null;
            }
            public ApplicationUser GetByName(String name)
            {
                ApplicationUser user = null;
                if (_mapNames.TryGetValue(name, out user))
                    return user;
                return null;
            }

            public ApplicationUser GetByEmail(String email)
            {
                ApplicationUser user = null;
                if (_mapEmails.TryGetValue(email, out user))
                    return user;
                return null;
            }

            public void CacheUser(ApplicationUser user)
            {
                if (user == null)
                    return;
                if (!_mapIds.ContainsKey(user.Id))
                {
                    _mapIds.Add(user.Id, user);
                }
                else
                {
                    var existing = _mapIds[user.Id];
                    if (!Comparer<ApplicationUser>.Equals(user, existing))
                        throw new InvalidProgramException("Invalid user cache");
                }
                if (!_mapNames.ContainsKey(user.UserName))
                {
                    _mapNames.Add(user.UserName, user);
                }
                if (!String.IsNullOrWhiteSpace(user.Email) && !_mapEmails.ContainsKey(user.Email))
                {
                    _mapEmails.Add(user.Email, user);
                }
                else
                {
                    var existing = _mapIds[user.Id];
                    if (!Comparer<ApplicationUser>.Equals(user, existing))
                        throw new InvalidProgramException("Invalid user cache");
                }
            }
        }

        IDbContext _dbContext;

        UserCache _cache;

        public ApplicationUserStore(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _cache = new UserCache();
        }

        #region IUserStore

        public async Task CreateAsync(ApplicationUser user)
        {
            await _dbContext.ExecuteAsync(null, "[Sec].[CreateUser]", user);
            CacheUser(user);
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByIdAsync(Int64 userId)
        {
            ApplicationUser user = _cache.GetById(userId);
            if (user != null)
                return user;
            user = await _dbContext.LoadAsync<ApplicationUser>(null, "[Sec].[FindUserById]", new { Id = userId });
            CacheUser(user);
            return user;
        }

        public async Task<ApplicationUser> FindByNameAsync(String userName)
        {
            ApplicationUser user = _cache.GetByName(userName);
            if (user != null)
                return user;
            user = await _dbContext.LoadAsync<ApplicationUser>(null, "[Sec].[FindUserByName]", new { UserName = userName });
            CacheUser(user);
            return user;
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            if (user.IsLockoutModified)
            {
                await _dbContext.ExecuteAsync<ApplicationUser>(null, "[Sec].[UpdateUserLockout]", user);
                user.ClearModified(UserModifiedFlag.Lockout);
            }
            else if (user.IsPasswordModified)
            {
                await _dbContext.ExecuteAsync<ApplicationUser>(null, "[Sec].[UpdateUserPassword]", user);
                user.ClearModified(UserModifiedFlag.Password);
            }
        }

        #endregion

        void CacheUser(ApplicationUser user)
        {
            _cache.CacheUser(user);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _cache = null;
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        #region IUserLoginStore
        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            IList<UserLoginInfo> list = new List<UserLoginInfo>();
            return Task.FromResult(list);
        }

        public Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserLockoutStore
        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return Task.FromResult<DateTimeOffset>(user.LockoutEndDateUtc);
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            if (user.LockoutEndDateUtc != lockoutEnd)
            {
                user.LockoutEndDateUtc = lockoutEnd;
                user.SetModified(UserModifiedFlag.Lockout);
            }
            return Task.FromResult<int>(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            user.AccessFailedCount += 1;
            user.SetModified(UserModifiedFlag.Lockout);
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            if (user.AccessFailedCount != 0)
            {
                user.AccessFailedCount = 0;
                user.SetModified(UserModifiedFlag.Lockout);
            }
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<Boolean> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, Boolean enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserPasswordStore
        public Task SetPasswordHashAsync(ApplicationUser user, String passwordHash)
        {
            user.PasswordHash = passwordHash;
            user.SetModified(UserModifiedFlag.Password);
            return Task.FromResult(0);
        }

        public Task<String> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<Boolean> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash != null);

        }
        #endregion

        #region IUserTwoFactorStore
        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }
        #endregion

        #region IUserEmailStore
        public Task SetEmailAsync(ApplicationUser user, String email)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<String> GetEmailAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<ApplicationUser> FindByEmailAsync(String email)
        {
            ApplicationUser user = _cache.GetByEmail(email);
            if (user != null)
                return user;
            user = await _dbContext.LoadAsync<ApplicationUser>(null, "[Sec].[FindUserByEmail]", new { Email = email });
            CacheUser(user);
            return user;
        }

        #endregion

        #region IUserPhoneNumberStore
        public Task SetPhoneNumberAsync(ApplicationUser user, String phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<String> GetPhoneNumberAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<Boolean> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserSecurityStampStore
        public Task SetSecurityStampAsync(ApplicationUser user, String stamp)
        {
            user.SecurityStamp = stamp;
            user.SetModified(UserModifiedFlag.Password);
            return Task.FromResult(0);
        }

        public Task<String> GetSecurityStampAsync(ApplicationUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
        #endregion

        #region IUserClaimStore 
        public Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            //TODO:
            /* добавляем все элементы, которые могут быть нужны БЕЗ загрузки объекта 
             * доступ через 
             * var user = HttpContext.Current.User.Identity as ClaimsIdentity;
             */
            IList<Claim> list = new List<Claim>();
            list.Add(new Claim("PersonName", user.PersonName ?? String.Empty));
            /*
			list.Add(new Claim("Locale", user.Locale ?? "uk_UA"));
			list.Add(new Claim("AppKey", user.ComputedAppKey));
			*/
            return Task.FromResult(list);
        }

        public Task AddClaimAsync(ApplicationUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(ApplicationUser user, Claim claim)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserRoleStore

        public Task AddToRoleAsync(ApplicationUser user, String roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, String roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<String>> GetRolesAsync(ApplicationUser user)
        {
            var list = await _dbContext.LoadListAsync<ApplicationRole>(null, "[Sec].[GetUserGroups]", new { UserId = user.Id });
            return list.Select<ApplicationRole, String>(x => x.Name).ToList();
        }

        public async Task<Boolean> IsInRoleAsync(ApplicationUser user, String roleName)
        {
            IList<String> roles = await GetRolesAsync(user);
            return roles.IndexOf(roleName) != -1;
        }
        #endregion
    }
}