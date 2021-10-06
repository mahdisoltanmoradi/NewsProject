using Data.Contract;
using Data.Repositories;
using DataLayer.Entities.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.Contracts
{
    public interface IPageRepository : IRepository<Page>
    {
        Task<IEnumerable<Page>> GetAllPage(CancellationToken cancellationToken);
        Task<IEnumerable<Page>> GetTopPage(CancellationToken cancellationToken, int take = 4);
        Task<IEnumerable<Page>> GetPagesinSlider(CancellationToken cancellationToken);
        Task<IEnumerable<Page>> GetLatesPage(CancellationToken cancellationToken);
        Task<IEnumerable<Page>> GetPagesByGroupId(int groupId, CancellationToken cancellationToken);
        Task<IEnumerable<Page>> Search(string q, CancellationToken cancellationToken);
        Task<Page> GetPageById(int pageId, CancellationToken cancellationToken);
        Task InsertPage(Page page, CancellationToken cancellationToken);
        Task UpdatePage(Page page, CancellationToken cancellationToken);
        Task<Page> DeletePage(int pageId, CancellationToken cancellationToken);
        Task<bool> PageExists(int pageId, CancellationToken cancellationToken);
        Task Save();
    }
}
