using DataLayer.Entities.Users;
using infrastructure.Services.PermissionHelper.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Services.PermissionHelper.ViewModel
{
    public class PermissionListViewModel
    {
        public int? RoleId { get; set; }
        public List<Role> Roles { get; set; }
        public List<PermissionTab> Tabs { get; set; } = new List<PermissionTab>();

        public SelectList GetRolesSelectList()
        {
            return new SelectList(Roles, nameof(Role.Id), nameof(Role.Name));
        }
    }
}
