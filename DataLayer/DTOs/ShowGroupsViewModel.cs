using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class ShowGroupsViewModel
    {
        public int GroupID { get; set; }
        public string GroupTitle { get; set; }
        public int PageCount { get; set; }
    }
}
