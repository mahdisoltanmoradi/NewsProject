using Data.Contract;
using DataLayer.DTOs.User;
using DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> IsExistUserName(string userName, CancellationToken cancellationToken);
        Task<bool> IsExistEmail(string email, CancellationToken cancellationToken);
        Task<User> IsExistEmailAndPassword(LoginViewModel login,CancellationToken cancellationToken);
        Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);
        Task<User> IsExistUserByUserEmail(string email, CancellationToken cancellationToken);
        Task<User> GetUserByUserName(string userName, CancellationToken cancellationToken);
        Task<int> GetUserIdByUserName(string userName, CancellationToken cancellationToken);
        Task<bool> ActiveAccount(string activeCode, CancellationToken cancellationToken);
        Task<User> GetUserByUserId(int userId, CancellationToken cancellationToken);
        Task<List<User>> GetAllUsers(CancellationToken cancellationToken);
       // Task UpdateUser(User user,CancellationToken cancellationToken);

    }
}
