using Common;
using Data;
using Data.Repositories;
using DataLayer.Contracts;
using DataLayer.Entities.Page;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.Repositorys
{
    public class PageRepository : Repository<Page>, IPageRepository,IScopedDependency
    {
        public PageRepository(ApplicationDbContext context)
            : base(context)
        {

        }


        public async Task<Page> DeletePage(int pageId, CancellationToken cancellationToken)
        {
            var page = await GetPageById(pageId, cancellationToken);
            await DeleteAsync(page, cancellationToken);
            return page;
        }

        public async Task<IEnumerable<Page>> GetAllPage(CancellationToken cancellationToken)
        {
            return await Table.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Page>> GetLatesPage(CancellationToken cancellationToken)
        {
            var page = await TableNoTracking.OrderByDescending(p => p.CreateDate).Take(4).ToListAsync(cancellationToken);
            return page;
        }

        public async Task<Page> GetPageById(int pageId, CancellationToken cancellationToken)
        {
            return await Table.SingleAsync(p => p.Id == pageId, cancellationToken);
        }

        public async Task<IEnumerable<Page>> GetPagesByGroupId(int groupId, CancellationToken cancellationToken)
        {
            var page = await TableNoTracking.Where(p => p.Id == groupId).ToListAsync(cancellationToken);
            return page;
        }

        public async Task<IEnumerable<Page>> GetPagesinSlider(CancellationToken cancellationToken)
        {
            return await TableNoTracking.Where(p => p.ShowInSlider).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Page>> GetTopPage(CancellationToken cancellationToken, int take = 4)
        {
            var page = await TableNoTracking.OrderByDescending(p => p.PageVisit).Take(take).ToListAsync(cancellationToken);
            return page;
        }

        public async Task InsertPage(Page page, CancellationToken cancellationToken)
        {
            await AddAsync(page,cancellationToken);
        }

        public Task<bool> PageExists(int pageId, CancellationToken cancellationToken)
        {
            return TableNoTracking.AnyAsync(p => p.Id == pageId, cancellationToken);
        }

        public Task Save()
        {
            return Save();
        }

        public async Task<IEnumerable<Page>> Search(string q, CancellationToken cancellationToken)
        {
            var list =await Table.Where(p =>
                 p.PageTitle.Contains(q) || p.ShortDescription.Contains(q) || p.PageText.Contains(q) ||
                 p.PageTags.Contains(q)).ToListAsync(cancellationToken);

            return list;
        }

        public async Task UpdatePage(Page page, CancellationToken cancellationToken)
        {
            await UpdateAsync(page, cancellationToken);
        }

    }
}
