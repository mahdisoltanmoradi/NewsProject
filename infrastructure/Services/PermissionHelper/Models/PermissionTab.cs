using System.Collections.Generic;

namespace infrastructure.Services.PermissionHelper.Models
{
    public class PermissionTab
    {
        public string Name { get; set; }
        public List<PermissionController> Controllers { get; set; } = new List<PermissionController>();
    }
}
