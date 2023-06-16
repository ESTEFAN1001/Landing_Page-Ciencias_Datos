using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LosCokis123.Data;
using LosCokis123.Models;
using System.Web;
using System.IO;
using SpotifyAPI.Web;

namespace LosCokis123.Controllers
{
    public class PodcastsController : Controller
    {
        private readonly LosCokis123Context _context;

        public PodcastsController(LosCokis123Context context)
        {
            _context = context;
        }

        // GET: Podcasts
        public async Task<IActionResult> Index()
        {
              return _context.Podcasts != null ? 
                          View(await _context.Podcasts.ToListAsync()) :
                          Problem("Entity set 'LosCokis123Context.Podcasts'  is null.");
        }
        public int NextID()
        {
            int maxId = _context.Podcasts.Max(e => e.Id);
            return maxId + 1;
        }

        // GET: Podcasts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Podcasts == null)
            {
                return NotFound();
            }

            var podcast = await _context.Podcasts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (podcast == null)
            {
                return NotFound();
            }

           return View(podcast);
        }

        // GET: Podcasts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Podcasts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Author,Image,CreateAt")] Podcast podcast, IFormFile image)
        {
            int nextID = NextID();
            podcast.CreateAt = DateTime.Now;
            string pathdb = "/podcast/" + nextID + ".jpg";
            podcast.Image = pathdb;

            if (ModelState.IsValid)
            {
                string path = "wwwroot/podcast/" + nextID + ".jpg";
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                _context.Add(podcast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(podcast);
        }

        // GET: Podcasts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Podcasts == null)
            {
                return NotFound();
            }

            var podcast = await _context.Podcasts.FindAsync(id);
            if (podcast == null)
            {
                return NotFound();
            }
            return View(podcast);
        }

        // POST: Podcasts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Author,Image,CreateAt")] Podcast podcast, IFormFile image)
        {
            if (id != podcast.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string path = "wwwroot/podcast/" + id + ".jpg";
                    string pathdb = "/podcast/" + id + ".jpg";
                    podcast.Image = pathdb;
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    _context.Update(podcast);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PodcastExists(podcast.Id))
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
            return View(podcast);
        }

        // GET: Podcasts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Podcasts == null)
            {
                return NotFound();
            }

            var podcast = await _context.Podcasts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (podcast == null)
            {
                return NotFound();
            }

            return View(podcast);
        }

        // POST: Podcasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Podcasts == null)
            {
                return Problem("Entity set 'LosCokis123Context.Podcasts'  is null.");
            }
            var podcast = _context.Podcasts.Include(p => p.Episodes)
                .ThenInclude(e => e.Comments)
                .Include(p => p.Episodes)
                .ThenInclude(e => e.Likes)
                .Include(p => p.Episodes)
                .ThenInclude(e => e.Favorites)
                .FirstOrDefault(p => p.Id == id);

            if (podcast != null)
            {
                _context.Podcasts.Remove(podcast);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PodcastExists(int id)
        {
          return (_context.Podcasts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
