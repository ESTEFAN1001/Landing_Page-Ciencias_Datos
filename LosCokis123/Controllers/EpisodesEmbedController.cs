using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LosCokis123.Data;
using LosCokis123.Models;

namespace LosCokis123.Controllers
{
    public class EpisodesEmbedController : Controller
    {
        private readonly LosCokis123Context _context;

        public EpisodesEmbedController(LosCokis123Context context)
        {
            _context = context;
        }

        // GET: EpisodesEmbed
        public async Task<IActionResult> Index()
        {
            var losCokis123Context = _context.Episodes.Include(e => e.Podcast);
            return View(await losCokis123Context.ToListAsync());
        }

        // GET: EpisodesEmbed/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: EpisodesEmbed/Create
        public IActionResult Create()
        {
            ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Title");
            return View();
        }

        // POST: EpisodesEmbed/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,PodcastId,Author,Duration,AudioUrl,CreateAt")] Episode episode)
        {
            episode.CreateAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(episode);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Episodes");
            }
            ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Title", episode.PodcastId);
            return View();
        }

        // GET: EpisodesEmbed/Edit/5
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

        // POST: EpisodesEmbed/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,PodcastId,Author,Duration,AudioUrl,CreateAt")] Episode episode)
        {
            if (id != episode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction("Index", "Episodes");
            }
            ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Id", episode.PodcastId);
            return View(episode);
        }

        // GET: EpisodesEmbed/Delete/5
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

        // POST: EpisodesEmbed/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Episodes == null)
            {
                return Problem("Entity set 'LosCokis123Context.Episodes'  is null.");
            }
            var episode = await _context.Episodes.FindAsync(id);
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
