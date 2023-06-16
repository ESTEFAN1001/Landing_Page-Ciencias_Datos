using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LosCokis123.Data;
using LosCokis123.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SpotifyAPI.Web;

namespace LosCokis123.Controllers
{
    public class EpisodesController : Controller
    {
        private readonly LosCokis123Context _context;

        public EpisodesController(LosCokis123Context context)
        {
            _context = context;
        }

        // GET: Episodes
        public async Task<IActionResult> Index()
        {
            var losCokis123Context = _context.Episodes.Include(e => e.Podcast);
            return View(await losCokis123Context.ToListAsync());
        }
        public int NextID()
        {
            int maxId = _context.Episodes.Max(e => e.Id);
            return maxId + 1;
        }

        // GET: Episodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Episodes == null)
            {
                return NotFound();
            }

            var episode = await _context.Episodes
                .Include(e => e.Podcast)
                .Include(e => e.Likes)
                .Include(e => e.Favorites)
                .Include(e => e.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (episode == null)
            {
                return NotFound();
            }

            return View(episode);
        }

        // GET: Episodes/Create
        public IActionResult Create()
        {
            ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Title");
            return View();
        }

        // POST: Episodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,PodcastId,Author,Duration,AudioUrl,CreateAt")] Episode episode, IFormFile audio)
        {
            int nextID = NextID();
            episode.CreateAt = DateTime.Now;
            string pathdb = "/episode/" + nextID + ".mp3";
            episode.AudioUrl = pathdb;

            if (ModelState.IsValid)
            {
                string path = "wwwroot/episode/" + nextID + ".mp3";
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await audio.CopyToAsync(stream);
                }
                _context.Add(episode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Title", episode.PodcastId);
            return View(episode);
        }
        // GET: Episodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Episodes == null)
            {
                return NotFound();
            }

            var episode = await _context.Episodes.FindAsync(id);
            if (episode == null)
            {
                return NotFound();
            }
            ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Title", episode.PodcastId);
            return View(episode);
        }

        // POST: Episodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,PodcastId,Author,Duration,AudioUrl,CreateAt")] Episode episode, IFormFile audio)
        {
            if (id != episode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string path = "wwwroot/episode/" + id + ".mp3";
                    string pathdb = "/episode/" + id + ".mp3";
                    episode.AudioUrl = pathdb;
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        await audio.CopyToAsync(stream);
                    }
                    _context.Update(episode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EpisodeExists(episode.Id))
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
            ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Id", episode.PodcastId);
            return View(episode);
        }

        // GET: Episodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Episodes == null)
            {
                return NotFound();
            }

            var episode = await _context.Episodes
                .Include(e => e.Podcast)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (episode == null)
            {
                return NotFound();
            }

            return View(episode);
        }

        // POST: Episodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Episodes == null)
            {
                return Problem("Entity set 'LosCokis123Context.Episodes'  is null.");
            }
            var episode = _context.Episodes.Include(e => e.Comments)
                .Include(e => e.Likes)
                .Include(e => e.Favorites)

            .FirstOrDefault(e => e.Id == id);
            if (episode != null)
            {
                _context.Episodes.Remove(episode);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EpisodeExists(int id)
        {
          return (_context.Episodes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
