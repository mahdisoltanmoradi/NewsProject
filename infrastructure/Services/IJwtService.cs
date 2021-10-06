using DataLayer.Entities.Users;
using Entities;
using System.Threading.Tasks;

namespace infrastructure.Services
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}