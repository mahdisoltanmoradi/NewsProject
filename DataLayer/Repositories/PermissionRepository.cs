using Common;
using Data;
using Data.Repositories;
using DataLayer.Contracts;
using DataLayer.DTOs.Permission;
using DataLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository, IScopedDependency
    {
        public PermissionRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public async Task AddPermissionToRole(int roleId, List<int> permission, CancellationToken cancellationToken)
        {
            foreach (var p in permission)
            {
                await DbContext.AddAsync(new Permission
                {
                    Id = p,
                    RoleId = roleId,
                }, cancellationToken);
            }
        }

        public async Task<List<Permission>> GetAllPermission(CancellationToken cancellationToken)
        {
            return await TableNoTracking.ToListAsync(cancellationToken);
        }

        public async Task<List<int>> PermissionRole(int roleId, CancellationToken cancellationToken)
        {
            return await Table.Where(pp => pp.RoleId == roleId)
                .Select(p => p.Id).ToListAsync(cancellationToken);
        }

        public async Task UpdatePermission(int roleId, List<int> permissions, CancellationToken cancellationToken)
        {
           var lists= await TableNoTracking.Where(p => p.RoleId == roleId).ToListAsync();
            lists.ForEach(p=>DbContext.Remove(p));
            await AddPermissionToRole(roleId, permissions, cancellationToken);
        }

        public async Task AddPermissionsIfNotExistsAsync(RolePermissionViewModel model)
        {
            foreach (var action in model.ActionFullNames)
            {
                var permission = new Permission
                {
                    RoleId = model.RoleId,
                    ActionFullName = action,
                };
                var exists = await TableNoTracking.AnyAsync(p => p.RoleId == permission.RoleId && p.ActionFullName == permission.ActionFullName);
                if (!exists)
                {
                    DbContext.Add(permission);
                }
            }
            await DbContext.SaveChangesAsync();
        }

        public async Task DeletePermissionsAsync(RolePermissionViewModel model)
        {
            var permissions = await Table.Where(p => p.RoleId == model.RoleId && model.ActionFullNames.Contains(p.ActionFullName)).ToListAsync();
            DbContext.RemoveRange(permissions);
            await DbContext.SaveChangesAsync();
        }

     

        public async Task<bool> UserHasPermissionAsync(int userId, string actionFullName)
        {
            var roles = await DbContext.UserRoles.Where(u=>u.UserId==userId).Select(r=>r.RoleId).ToListAsync();
            var permissions = await Table.Select(p => new { p.RoleId, p.ActionFullName }).ToListAsync();

            var hasPermission = permissions.Any(p => roles.Contains(p.RoleId) &&
                 p.ActionFullName.Equals(actionFullName, StringComparison.OrdinalIgnoreCase));

            return hasPermission;
        }
    }
}
