using Data.Contract;
using DataLayer.DTOs;
using DataLayer.Entities.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.Contracts
{
    public interface IPageGroupRepository : IRepository<PageGroup>
    {
        Task<List<PageGroup>> GetAllPageGroups(CancellationToken cancellationToken);
        Task<PageGroup> GetPageGroupById(int groupId, CancellationToken cancellationToken);
        Task InsertPageGroup(PageGroup pageGroup, CancellationToken cancellationToken);
        Task UpdatePageGroup(PageGroup pageGroup, CancellationToken cancellationToken);
        Task DeletePageGroup(PageGroup pageGroup, CancellationToken cancellationToken);
        Task DeletePageGroup(int groupId, CancellationToken cancellationToken);
        Task<bool> PageGroupExists(int pageGroupId, CancellationToken cancellationToken);
        Task<List<ShowGroupsViewModel>> GetListGroups(CancellationToken cancellationToken);
        Task Save();
    }
}
