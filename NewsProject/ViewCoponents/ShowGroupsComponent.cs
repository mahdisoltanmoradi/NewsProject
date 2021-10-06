using DataLayer.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsProject.ViewCoponents
{
    public class ShowGroupsComponent:ViewComponent
    {
        private IPageGroupRepository _pageGroupRepository;

        public ShowGroupsComponent(IPageGroupRepository pageGroupRepository)
        {
            this._pageGroupRepository = pageGroupRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            return View("ShowGroupsComponent", await _pageGroupRepository.GetListGroups(cancellationToken));
        }
     
    }
}
