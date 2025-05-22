using PayRollManagement.DTO;
using PayRollManagement.Models;

namespace PayRollManagement.Interface
{
    public interface IUserRepository
    {
        Task<int> RegisterUser(UserRegistrationDto dto);
        Task<User> LoginUser(LoginDto dto);
    }
}
