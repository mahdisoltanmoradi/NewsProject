using DataLayer.Contracts;
using DataLayer.Entities.Page;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PageController : Controller
    {
        private IPageRepository _pageRepoitory;
        private IPageGroupRepository _pageGroupRepository;

        public PageController(IPageRepository pageRepoitory, IPageGroupRepository pageGroupRepository)
        {
            _pageRepoitory = pageRepoitory;
            _pageGroupRepository = pageGroupRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var myCmsDbContext = await _pageRepoitory.GetAllPage(cancellationToken);
            return View(myCmsDbContext);
        }

        public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }
            var page = await _pageRepoitory.GetPageById(id.Value, cancellationToken);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            ViewBag.PageGroupId = new SelectList(await _pageGroupRepository.GetAllPageGroups(cancellationToken), "Id", "GroupTitle", 0);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Page page, IFormFile imgup, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                page.PageVisit = 0;
                page.CreateDate = DateTime.Now;

                if (imgup != null)
                {
                    page.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgup.FileName);
                    string savePath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/PageImages", page.ImageName
                    );
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await imgup.CopyToAsync(stream);
                    }
                }

                await _pageRepoitory.AddAsync(page, cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.GroupID = new SelectList(await _pageGroupRepository.GetAllPageGroups(cancellationToken), "Id", "GroupTitle", page.GroupID);
            return View(page);
        }

        public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }
            var page = await _pageRepoitory.GetPageById(id.Value, cancellationToken);
            if (page == null)
            {
                return NotFound();
            }
            ViewBag.GroupID = new SelectList(await _pageGroupRepository.GetAllPageGroups(cancellationToken), "Id", "GroupTitle", page.GroupID);
            return View(page);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Page page, IFormFile imgup, CancellationToken cancellationToken)
        {
            if (id != page.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (imgup != null)
                    {
                        if (page.ImageName == null)
                        {
                            page.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgup.FileName);
                        }

                        string savePath = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/PageImages", page.ImageName
                        );

                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await imgup.CopyToAsync(stream);
                        }

                    }
                    await _pageRepoitory.UpdateAsync(page, cancellationToken);
}

                catch
                {
                    if (_pageRepoitory.TableNoTracking.Any(p => p.Id == id))
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
            ViewBag.GroupID = new SelectList(await _pageGroupRepository.GetAllPageGroups(cancellationToken), "Id", "GroupTitle", page.GroupID);
            return View(page);
        }

        public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _pageRepoitory.GetPageById(id.Value, cancellationToken);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var page = await _pageRepoitory.GetPageById(id, cancellationToken);
            await _pageRepoitory.DeleteAsync(page, cancellationToken);

            if (page.ImageName != null)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PageImages", page.ImageName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            await _pageRepoitory.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}

