using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs.Permission
{
    public class RolePermissionViewModel
    {
        public int RoleId { get; set; }
        public List<string> ActionFullNames { get; set; }
    }
}
