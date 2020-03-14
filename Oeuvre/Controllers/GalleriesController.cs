using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Oeuvre.Models;
//Test to see if I know what I am doing with Github
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
            int galCount = _context.Gallery.Count();
            //Variable Declaration
             List<Gallery> galleryList = new List<Gallery>();
            galleryList = await _context.Gallery.ToListAsync();
            string id;
            // galleryList.Equals();
            //List<int> galleryIDs = new List<int>();
            // return View(await _context.Gallery.Include(a => a.Image).ToListAsync());
            //List<GalleryDisplay> galleries = new List<GalleryDisplay>();
            //GalleryDisplay galleryImages = new GalleryDisplay();
            List<string> tempName = new List<string>();
            List<string> tempAddress = new List<string>();
            List<string> tempProvince = new List<string>();
            List<string> tempPostal = new List<string>();
            List<string> tempCity= new List<string>();
            List<string> tempId = new List<string>();
            List < List < Image >> tempImages = new List<List<Image>>();
            GalleryListing galleryCollection = new GalleryListing();
            List<Image> myList = new List<Image>();
            //How many Galleries there are
            Console.WriteLine(galCount);
            int idCheck = 0;
            int idCheckAdv=1;
            var gallery = await _context.Gallery.FirstOrDefaultAsync();







            for (int x =0; x<galCount; x++)
            {
                
                /*for(int y = idCheck; y < idCheckAdv; y++)
                  {
                      while (gallery == null)
                      {
                          try
                          {
                              gallery = await _context.Gallery
                     .FirstOrDefaultAsync(m => m.GalleryId == y.ToString());
                          }
                          catch (Exception e)
                          {

                          }
                          idCheck += 1;
                          idCheckAdv += 1;
                      }
                  }
                  id = idCheck.ToString();
                  */
                id = galleryList[x].GalleryId.ToString();
                gallery = await _context.Gallery
                .FirstOrDefaultAsync(m => m.GalleryId == int.Parse(id));
                tempId.Add(gallery.GalleryId.ToString());
                tempName.Add(gallery.GalleryName);
                tempAddress.Add( gallery.Address);
                tempProvince.Add(gallery.Province);               
                tempPostal.Add(gallery.PostalCode);
                tempCity.Add( gallery.City);

                var images = (from image in _context.Image
                              where image.GalleryId == int.Parse(id)
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
                for (int i = 1; i <= 3; i++)
                {
                    bool testExsist = true;
                    Image tempImage = new Image();
                    try
                    {
                        tempImage.ImgLocation = images.ElementAt(i).ImgLocation;
                        testExsist = true;
                    }
                    catch(Exception q)
                    {
                        Console.Out.WriteLine(q);
                        testExsist = false;
                    }

                    if(!testExsist)
                    {
                        tempImage.ImgLocation = "https://res.cloudinary.com/oeuvre/image/upload/v1583410971/no-image-available_jkydpu.jpg";
                        myList.Add(tempImage);
                       // tempImages.Add(myList);
                        testExsist = true;
                    }
                    else
                    {
                        //tempImage.ImgLocation = "https://res.cloudinary.com/oeuvre/image/upload/v1583410971/no-image-available_jkydpu.jpg";

                        tempImage.ImgLocation = images.ElementAt(i).ImgLocation;
                        myList.Add(tempImage);
                       
                        testExsist = true;
                    }
                    
                    if (i == 3)
                    {
                        tempImages.Add(myList);
                        myList = new List<Image>();
                    }
                    
                }
                galleryCollection.GalleryId = tempId;
                galleryCollection.GalleryName = tempName;
                galleryCollection.Address = tempAddress;
                galleryCollection.Province= tempProvince;
                galleryCollection.PostalCode = tempPostal;
                galleryCollection.City = tempCity;
                galleryCollection.Images=tempImages;
               // idCheckAdv = idCheck + 1;
                //galleryCollection.galleryDisplays.Add(galleryImages);


            }



            //get the images code =)
            // id = "0";
               
               

            
            return View(galleryCollection);
        }

        // GET: Galleries/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gallery = await _context.Gallery
                .FirstOrDefaultAsync(m => m.GalleryId == int.Parse(id));
            if (gallery == null)
            {
                return NotFound();
            }
            List<Image> myList = new List<Image>();



            var images = (from image in _context.Image
                          where image.GalleryId == int.Parse(id)
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

            for (int i = 0; i < images.Count; i++)
            {
                Image tempImage = new Image();
                tempImage.ImgLocation = images.ElementAt(i).ImgLocation;
                myList.Add(tempImage);
            }

            GalleryDisplay galleryImages = new GalleryDisplay();
            galleryImages.GalleryName = gallery.GalleryName;
            galleryImages.Address = gallery.Address;
            galleryImages.Province = gallery.Province;
            galleryImages.PostalCode = gallery.PostalCode;
            galleryImages.City = gallery.City;
            galleryImages.Images = myList;

            
            return View(galleryImages);
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
            if (int.Parse(id) != gallery.GalleryId)
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
                    if (!GalleryExists(gallery.GalleryId.ToString()))
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
                .FirstOrDefaultAsync(m => m.GalleryId == int.Parse(id));
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
            return _context.Gallery.Any(e => e.GalleryId == int.Parse(id));
        }
    }
}
