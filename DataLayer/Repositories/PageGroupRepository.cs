using Common;
using Data;
using Data.Repositories;
using DataLayer.Contracts;
using DataLayer.DTOs;
using DataLayer.Entities.Page;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.Repositorys
{
    public class PageGroupRepository :Repository<PageGroup>,IPageGroupRepository, IScopedDependency
    {
        public PageGroupRepository(ApplicationDbContext context)
            :base(context)
        {

        }

        public async Task DeletePageGroup(PageGroup pageGroup, CancellationToken cancellationToken)
        {
           await DeleteAsync(pageGroup,cancellationToken);
        }

        public async Task DeletePageGroup(int groupId, CancellationToken cancellationToken)
        {
            var pageGroup = await GetPageGroupById(groupId,cancellationToken);
            await DeleteAsync(pageGroup, cancellationToken);            
        }

        public async Task<List<PageGroup>> GetAllPageGroups(CancellationToken cancellationToken)
        {
            return await Table.ToListAsync(cancellationToken);
        }

        public async Task<List<ShowGroupsViewModel>> GetListGroups(CancellationToken cancellationToken)
        {
            return await TableNoTracking.Select(g => new ShowGroupsViewModel()
            {
                GroupID = g.Id,
                GroupTitle = g.GroupTitle,
                PageCount = g.Pages.Count()
            }).ToListAsync();
        }

        public async Task<PageGroup> GetPageGroupById(int groupId, CancellationToken cancellationToken)
        {
            var pageGroup = await Table.SingleOrDefaultAsync(p=>p.Id==groupId);
            return pageGroup;
           
        }

        public async Task InsertPageGroup(PageGroup pageGroup, CancellationToken cancellationToken)
        {
            await AddAsync(pageGroup,cancellationToken);
        }

        public async Task<bool> PageGroupExists(int pageGroupId, CancellationToken cancellationToken)
        {
            return await TableNoTracking.AnyAsync(p => p.Id == pageGroupId,cancellationToken);
        }

        public async Task Save()
        {
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdatePageGroup(PageGroup pageGroup, CancellationToken cancellationToken)
        {
            await UpdateAsync(pageGroup,cancellationToken);
        }
    }
}
