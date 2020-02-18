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
    public class GalleriesController : Controller
    {
        private readonly dbo_OeuvreContext _context;

        public GalleriesController(dbo_OeuvreContext context)
        {
            _context = context;
        }

        // GET: Galleries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gallery.ToListAsync());
        }

        // GET: Galleries/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gallery = await _context.Gallery
                .FirstOrDefaultAsync(m => m.GalleryId == id);
            if (gallery == null)
            {
                return NotFound();
            }
            List<Image> myList = new List<Image>();
            

            var images = (from image in _context.Image
                          where image.GalleryId.Contains(id)
                          select new
                          {
                              image.ImgId,
                              image.ImgLocation,
                              image.GalleryId,
                              image.ThemeId,
                              image.DateUploaded,
                              image.Artist,
                              image.Description,
                              image.Name

                          }).ToList();

            GalleryDisplay galleryImages = new GalleryDisplay();
           // galleryImages.GalleryName = galleryName;
           // galleryImages.Address = address;
           // galleryImages.Images = images;


            return View(gallery);
        }

        // GET: Galleries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Galleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GalleryId,GalleryName,Address,City,PostalCode,Province,AuthUserId")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gallery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gallery);
        }

        // GET: Galleries/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gallery = await _context.Gallery.FindAsync(id);
            if (gallery == null)
            {
                return NotFound();
            }
            return View(gallery);
        }

        // POST: Galleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("GalleryId,GalleryName,Address,City,PostalCode,Province,AuthUserId")] Gallery gallery)
        {
            if (id != gallery.GalleryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gallery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryExists(gallery.GalleryId))
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
            return View(gallery);
        }

        // GET: Galleries/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gallery = await _context.Gallery
                .FirstOrDefaultAsync(m => m.GalleryId == id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // POST: Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gallery = await _context.Gallery.FindAsync(id);
            _context.Gallery.Remove(gallery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalleryExists(string id)
        {
            return _context.Gallery.Any(e => e.GalleryId == id);
        }
    }
}
