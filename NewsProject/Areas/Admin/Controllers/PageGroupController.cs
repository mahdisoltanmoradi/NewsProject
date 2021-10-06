using DataLayer.Contracts;
using DataLayer.Entities.Page;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PageGroupController : Controller
    {
        private IPageGroupRepository _pageGroup;
        public PageGroupController(IPageGroupRepository pageGroup)
        {
            this._pageGroup = pageGroup;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _pageGroup.GetAllPageGroups(cancellationToken));
        }

        public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageGroup = await _pageGroup.GetPageGroupById(id.Value, cancellationToken);
            if (pageGroup == null)
            {
                return NotFound();
            }

            return View(pageGroup);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PageGroup pageGroup, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                await _pageGroup.AddAsync(pageGroup, cancellationToken);
                return Redirect(nameof(Index));
            }
            return View(pageGroup);
        }


        public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageGroup = await _pageGroup.GetPageGroupById(id.Value, cancellationToken);
            if (pageGroup == null)
            {
                return NotFound();
            }
            return View(pageGroup);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PageGroup pageGroup, CancellationToken cancellationToken)
        {
            if (id != pageGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _pageGroup.UpdateAsync(pageGroup, cancellationToken);
                }
                catch
                {
                    if (_pageGroup.TableNoTracking.Any(o => o.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pageGroup);
        }


        public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageGroup = await _pageGroup.GetPageGroupById(id.Value, cancellationToken);
            if (pageGroup == null)
            {
                return NotFound();
            }

            return View(pageGroup);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id, CancellationToken cancellationToken)
        {
            var page= await _pageGroup.GetByIdAsync(cancellationToken,Id);
            await _pageGroup.DeleteAsync(page,cancellationToken);
            return RedirectToAction(nameof(Index));
        }

    }
}

