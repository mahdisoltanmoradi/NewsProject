using Data.Contract;
using DataLayer.Contracts;
using DataLayer.DTOs.Permission;
using DataLayer.Entities.Users;
using infrastructure.Services.Attributes;
using infrastructure.Services.PermissionHelper;
using infrastructure.Services.PermissionHelper.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ControllerInfo("دسترسی ها", "احراز هویت")]
    public class PermissionController : Controller,ICheckController
    {
        private readonly IPermissionRepository _permissionRepository;

        private readonly IRepository<Role> _roleRepository;
        public PermissionController(IPermissionRepository permissionRepository, IRepository<Role> roleRepository)
        {
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
        }

        public async Task<IActionResult> Index(int id=0)
        {
            var allRoles = await _roleRepository.Table.ToListAsync();
            var model = new PermissionListViewModel { Roles = allRoles };

            var role = await _roleRepository.Table.Include(p => p.Permissions).SingleOrDefaultAsync(p=>p.Id==id);
            if (role != null)
            {
                model.RoleId = role.Id;
                model.Tabs = PermissionHelper.GetPermissionTabViewModels(role.Permissions);
            }
            return View(model);
        }

        [HttpPost]
        [ActionInfo("تغییر سطوح دسترسی")]
        public async Task<ActionResult> AddPermission(RolePermissionViewModel model)
        {
            await _permissionRepository.AddPermissionsIfNotExistsAsync(model);
            return Json(new { result = true });
        }

        [HttpPost]
        [ActionInfo("تغییر سطوح دسترسی")]
        public async Task<ActionResult> DeletePermission(RolePermissionViewModel model)
        {
            await _permissionRepository.DeletePermissionsAsync(model);
            return Json(new { result = true });
        }
    }
}
