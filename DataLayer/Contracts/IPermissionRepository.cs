using Data.Contract;
using DataLayer.DTOs.Permission;
using DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.Contracts
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<List<Permission>> GetAllPermission(CancellationToken cancellationToken);
        Task AddPermissionToRole(int roleId, List<int> permission, CancellationToken cancellationToken);
        Task<List<int>> PermissionRole(int roleId, CancellationToken cancellationToken);
        Task UpdatePermission(int roleId, List<int> permissions, CancellationToken cancellationToken);

        Task AddPermissionsIfNotExistsAsync(RolePermissionViewModel model);
        Task DeletePermissionsAsync(RolePermissionViewModel model);
        //Task AddRangeAsync(IEnumerable<Permission> permissions);
        Task<bool> UserHasPermissionAsync(int userId, string actionFullName);
    }
}
