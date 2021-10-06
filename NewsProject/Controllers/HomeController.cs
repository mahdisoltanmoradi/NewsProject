using Data.Contract;
using DataLayer.Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsProject.Controllers
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class HomeController : Controller
    {
        private IPageRepository _pageRepository;

        public HomeController(IPageRepository pageRepository)
        {
            this._pageRepository = pageRepository;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            ViewData["Slider"] =await _pageRepository.GetPagesinSlider(cancellationToken);
            return View(await _pageRepository.GetLatesPage(cancellationToken));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
