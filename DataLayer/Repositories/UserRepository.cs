using Common;
using Common.Utilities.Convertors;
using Data;
using Data.Repositories;
using DataLayer.Contracts;
using DataLayer.DTOs.User;
using DataLayer.Entities.Page;
using DataLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class UserRepository:Repository<User>,IUserRepository,IScopedDependency
    {
        public UserRepository(ApplicationDbContext context)
            :base(context)
        {

        }

        public async Task<bool> ActiveAccount(string activeCode, CancellationToken cancellationToken)
        {
            return await TableNoTracking.AnyAsync(p => p.ActiveCode == activeCode);
        }

        public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken)
        {
            return await TableNoTracking.ToListAsync(cancellationToken);
        }

        public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            return await TableNoTracking.SingleOrDefaultAsync(e => e.Email == email,cancellationToken);
        }

        public async Task<User> GetUserByUserId(int userId, CancellationToken cancellationToken)
        {
            return await TableNoTracking.SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<User> GetUserByUserName(string userName, CancellationToken cancellationToken)
        {
            return await TableNoTracking.SingleOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        }

        public async Task<int> GetUserIdByUserName(string userName, CancellationToken cancellationToken)
        {
            var user=await TableNoTracking.SingleAsync(u => u.UserName == userName, cancellationToken);
            return user.Id;
        }

        public async Task<bool> IsExistEmail(string email, CancellationToken cancellationToken)
        {
            return await TableNoTracking.AnyAsync(u => u.Email == email, cancellationToken);    
        }

        public async Task<User> IsExistEmailAndPassword(LoginViewModel login,CancellationToken cancellationToken)
        {
            string FixeEmail = FixedText.FixeEmail(login.Email);
            string hashPassword =login.Password;
            return await TableNoTracking.SingleOrDefaultAsync(u=>u.Email==login.Email&&u.Password==hashPassword,cancellationToken);
        }

        public async Task<User> IsExistUserByUserEmail(string email, CancellationToken cancellationToken)
        {
            return await TableNoTracking.SingleOrDefaultAsync(u => u.Email == email,cancellationToken);
        }

        public async Task<bool> IsExistUserName(string userName, CancellationToken cancellationToken)
        {
            return await TableNoTracking.AnyAsync(u => u.UserName == userName, cancellationToken);
        }
    }
}
