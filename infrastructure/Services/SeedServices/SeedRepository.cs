using Data.Contract;
using DataLayer.Contracts;
using DataLayer.DTOs.User;
using DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using infrastructure.Services.PermissionHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common;

namespace infrastructure.Services.SeedSevices
{
    public class SeedRepository : ISeedRepository,IScopedDependency
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public SeedRepository( IPermissionRepository permissionRepository,RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            this._permissionRepository = permissionRepository;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            if (await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new Role {Name="Admin" });
            }
            if (await _userManager.Users.AsNoTracking().AnyAsync(u=>u.UserName=="admin"))
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "site@example.com"
                };
                var res= await _userManager.CreateAsync(user);
                if (res.Succeeded)
                   await _userManager.AddToRoleAsync(user,"Admin");
            }

            var allActionFullNames = PermissionHelper.PermissionHelper.Tabs
                .SelectMany(tab => tab.Controllers.SelectMany(controller => controller.Actions.SelectMany(action => action.FullNames)))
                .Distinct().ToList();

            var permissions = allActionFullNames.Select(p => new Permission { ActionFullName = p, RoleId = 1 }).ToList();
            await _permissionRepository.AddRangeAsync(permissions,new CancellationTokenSource().Token);
        }
    }
}

