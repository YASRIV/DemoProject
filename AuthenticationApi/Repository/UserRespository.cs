using Car_Wash_Library.DTOClass;
using System.Net.Http;
using Car_Wash_Library.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Repository
{
    public class UserRespository
    {
        public interface IUserRepository
        {
            Task<User> Authenticate(AuthRequest request);
            Task<Role> GetRoleForUser(int userId);
            Task<User> GetUserByEmail(string email);
            Task<bool> RegisterUser(User user, string role);
            bool IsEmailExists(string email);

        }

        public class UsersRepository : IUserRepository
        {
            private readonly AuthDbContext _context;

            public UsersRepository(AuthDbContext context)
            {
                _context = context;
            }
            public async Task<User> Authenticate(AuthRequest request)
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == request.Password);
            }
            public async Task<Role> GetRoleForUser(int userId)
            {
                return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId ==
                    _context.UserRoles.FirstOrDefault(ur => ur.UserId == userId).RoleId);
            }
            public async Task<User> GetUserByEmail(string email)
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }


            //user repo methods 
            public bool IsEmailExists(string email)

            {

                return _context.Users.Any(u => u.Email == email);

            }

            public async Task<bool> RegisterUser(User user, string role)

            {

                try

                {
                    if (user == null) return false;

                    _context.Users.Add(user);

                    await _context.SaveChangesAsync();

                    int roleId = 0;

                    if (role == "Admin") roleId = 1;

                    else if (role == "Customer") roleId = 2;

                    else if (role == "Washer") roleId = 3;

                    var userrole = new UserRole

                    {

                        UserId = user.UserId,

                        RoleId = roleId,

                        IsActive = true

                    };

                    _context.UserRoles.Add(userrole);

                    await _context.SaveChangesAsync();

                    return true;

                }

                catch (Exception ex)

                {

                    throw;

                }

            }



        }
    }
}