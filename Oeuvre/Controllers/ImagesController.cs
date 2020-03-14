using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Oeuvre.Models;

namespace Oeuvre.Controllers
{
    public class ImagesController : Controller
    {
        private readonly dbo_OeuvreContext _context;

        public ImagesController(dbo_OeuvreContext context)
        {
            _context = context;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            var dbo_OeuvreContext = _context.Image.Include(i => i.Gallery).Include(i => i.Theme);
            return View(await dbo_OeuvreContext.ToListAsync());
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Image
                .Include(i => i.Gallery)
                .Include(i => i.Theme)
                .FirstOrDefaultAsync(m => m.ImgId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            ViewData["GalleryId"] = new SelectList(_context.Gallery, "GalleryId", "GalleryId");
            ViewData["ThemeId"] = new SelectList(_context.Theme, "ThemeId", "ThemeId");
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImgId,GalleryId,DateUploaded,ThemeId,Description,ImgLocation,Artist,Name")] Image image)
        {
            if (ModelState.IsValid)
            {
                _context.Add(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GalleryId"] = new SelectList(_context.Gallery, "GalleryId", "GalleryId", image.GalleryId);
            ViewData["ThemeId"] = new SelectList(_context.Theme, "ThemeId", "ThemeId", image.ThemeId);
            return View(image);
        }

        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Image.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            ViewData["GalleryId"] = new SelectList(_context.Gallery, "GalleryId", "GalleryId", image.GalleryId);
            ViewData["ThemeId"] = new SelectList(_context.Theme, "ThemeId", "ThemeId", image.ThemeId);
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ImgId,GalleryId,DateUploaded,ThemeId,Description,ImgLocation,Artist,Name")] Image image)
        {
            if (id != image.ImgId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.ImgId))
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
            ViewData["GalleryId"] = new SelectList(_context.Gallery, "GalleryId", "GalleryId", image.GalleryId);
            ViewData["ThemeId"] = new SelectList(_context.Theme, "ThemeId", "ThemeId", image.ThemeId);
            return View(image);
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Image
                .Include(i => i.Gallery)
                .Include(i => i.Theme)
                .FirstOrDefaultAsync(m => m.ImgId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var image = await _context.Image.FindAsync(id);
            _context.Image.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(string id)
        {
            return _context.Image.Any(e => e.ImgId == id);
        }
    }
}
