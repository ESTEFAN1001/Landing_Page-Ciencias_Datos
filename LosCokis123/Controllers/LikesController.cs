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
    public class LikesController : Controller
    {
        private readonly LosCokis123Context _context;

        public LikesController(LosCokis123Context context)
        {
            _context = context;
        }

        // GET: Likes
        public async Task<IActionResult> Index()
        {
            var losCokis123Context = _context.Likes.Include(l => l.Episode);
            return View(await losCokis123Context.ToListAsync());
        }

        // GET: Likes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Likes == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Episode)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // GET: Likes/Create
        public IActionResult Create()
        {
            ViewData["EpisodeId"] = new SelectList(_context.Episodes, "Id", "Id");
            return View();
        }

        // POST: Likes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string user_id, int episode_id, int like_id ,[Bind("Id,Likeop,EpisodeId,UserId,CreateAt")] Like like)
        {
            var result = await _context.Likes.FirstOrDefaultAsync(l => l.Id == like_id);
            if (result == default)
            {
                like.UserId = user_id;
                like.EpisodeId = episode_id;
                like.Likeop = 2;
                like.CreateAt = DateTime.Now;
                _context.Add(like);
            }
            else 
            {
                result.Likeop = result.Likeop == 1? 2 : 1;
                _context.Update(result);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Episodes", new { id = episode_id });
        }

        // GET: Likes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Likes == null)
            {
                return NotFound();
            }

            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }
            ViewData["EpisodeId"] = new SelectList(_context.Episodes, "Id", "Id", like.EpisodeId);
            return View(like);
        }

        // POST: Likes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Likeop,EpisodeId,UserId,CreateAt")] Like like)
        {
            if (id != like.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(like);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikeExists(like.Id))
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
            ViewData["EpisodeId"] = new SelectList(_context.Episodes, "Id", "Id", like.EpisodeId);
            return View(like);
        }

        // GET: Likes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Likes == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Episode)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Likes == null)
            {
                return Problem("Entity set 'LosCokis123Context.Likes'  is null.");
            }
            var like = await _context.Likes.FindAsync(id);
            if (like != null)
            {
                _context.Likes.Remove(like);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LikeExists(int id)
        {
          return (_context.Likes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
