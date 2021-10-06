using DataLayer.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsProject.ViewCoponents
{
    public class ShowTopPageComponent : ViewComponent
    {
        private IPageRepository _pageRepository;

        public ShowTopPageComponent(IPageRepository pageRepository)
        {
            this._pageRepository = pageRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            return View("ShowTopPageComponent", await _pageRepository.GetTopPage(cancellationToken));
        }

    }
}
