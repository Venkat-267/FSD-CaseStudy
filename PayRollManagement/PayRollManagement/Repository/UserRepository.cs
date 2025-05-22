using Microsoft.EntityFrameworkCore;
using PayRollManagement.DTO;
using PayRollManagement.Interface;
using PayRollManagement.Models;
using System.Security.Cryptography;
using System.Text;

namespace PayRollManagement.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly PayMasterDbContext _context;
        public UserRepository(PayMasterDbContext context)
        {
            _context = context; // Dependency Injection to Use DbContext
        }

        private string ComputeHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<int> RegisterUser(UserRegistrationDto dto)
        {
            if(await _context.Users.AnyAsync(u=> u.UserName == dto.Username))
            {
                throw new Exception("Username Already Exists!");
            }

            var passwordHash = ComputeHash(dto.Password);

            var user = new User { UserName = dto.Username, Email = dto.Email, Password = passwordHash, RoleId = dto.RoleId, IsActive = true, CreatedAt = DateTime.Now };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.UserId;
        }

        public async Task<User> LoginUser(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.Username);

            if(user == null || user.Password != ComputeHash(dto.Password))
            {
                throw new Exception("Invalid Credentials!");
            }

            return user;
        }
    }
}
