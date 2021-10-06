using DataLayer.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsProject.Controllers
{
    [AllowAnonymous]
    public class NewsController : Controller
    {
        IPageRepository _pageRepository;
        public NewsController(IPageRepository pageRepository)
        {
            this._pageRepository = pageRepository;
        }

        [Route("News/{newsId}")]
        public async Task<IActionResult> ShowNews(int newsId,CancellationToken cancellationToken)
        {
            var page =await _pageRepository.GetPageById(newsId,cancellationToken);

            if (page !=null)
            {
                page.PageVisit += 1;
                await _pageRepository.UpdateAsync(page,cancellationToken);
            }

            return View(page);
        }

        [Route("Group/{groupId}/{title}")]
        public async Task<IActionResult> ShowNewsByGroupId(int groupId, string title,CancellationToken cancellationToken)
        {
            ViewData["GroupTitle"] = title;
            return View(await _pageRepository.GetPagesByGroupId(groupId,cancellationToken));
        }

        [Route("Search")]
        public async Task<IActionResult> Search(string q,CancellationToken cancellationToken)
        {
            return View(await _pageRepository.Search(q,cancellationToken));
        }
    }
}
