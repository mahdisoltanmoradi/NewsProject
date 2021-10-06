using System.Collections.Generic;

namespace infrastructure.Services.PermissionHelper.Models
{
    public class PermissionAction
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
        public List<string> FullNames { get; set; }
    }
}
