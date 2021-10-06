using AutoMapper;
using Data.Contract;
using DataLayer.Contracts;
using DataLayer.Entities.Users;
using infrastructure.Services.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ControllerInfo("نقش ها", "احراز هویت")]
    public class RoleController : Controller
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public RoleController(IRepository<Role> RoleRepository, IPermissionRepository permissionRepository, IMapper mapper)
        {
            this._roleRepository = RoleRepository;
            this._permissionRepository = permissionRepository;
            this._mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var roleList = await _roleRepository.Table.ToListAsync();
            return View(roleList);
        }

        public async Task<IActionResult> CreateRole(CancellationToken cancellationToken)
        {
            ViewData["Permission"] = await _permissionRepository.GetAllPermission(cancellationToken);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(Role role, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            role.IsDelete = false;
            await _roleRepository.AddAsync(role, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteRole(int roleId, CancellationToken cancellationToken)
        {
            if (roleId==null)
            {
                return NotFound();
            }
            var roles = await _roleRepository.GetByIdAsync(cancellationToken, roleId);
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(Role role, CancellationToken cancellationToken)
        {
            await _roleRepository.DeleteAsync(role, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

    }
}
