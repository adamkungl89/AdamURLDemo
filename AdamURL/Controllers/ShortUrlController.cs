using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdamURL.Data;
using AdamURL.Models;

namespace AdamURL.Controllers
{
    public class ShortUrlController : Controller
    {
        private readonly ShortUrlContext _context;

        public ShortUrlController(ShortUrlContext context)
        {
            _context = context;
        }

        private bool loggedIn()
        {
            string loginVal;
            if (Request.Cookies.TryGetValue("adamUrlLogin", out loginVal) && loginVal == "admin")
                return true;
            return false;
        }

        // GET: ShortUrl
        public async Task<IActionResult> Index()
        {
            if (!loggedIn()) return View("Login");
            return View(await _context.ShortUrl.ToListAsync()); 
        }

        // GET: ShortUrl/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!loggedIn()) return View("Login");

            if (id == null)
            {
                return NotFound();
            }

            var shortUrl = await _context.ShortUrl
                .FirstOrDefaultAsync(m => m.id == id);
            if (shortUrl == null)
            {
                return NotFound();
            }

            return View(shortUrl);
        }

        // GET: ShortUrl/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!loggedIn()) return View("Login");

            if (id == null)
            {
                return NotFound();
            }

            var shortUrl = await _context.ShortUrl.FindAsync(id);
            if (shortUrl == null)
            {
                return NotFound();
            }
            return View(shortUrl);
        }

        // POST: ShortUrl/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,longUrl,key")] ShortUrl shortUrl)
        {
            if (!loggedIn()) return View("Login");

            if (id != shortUrl.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shortUrl);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShortUrlExists(shortUrl.id))
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
            return View(shortUrl);
        }

        // GET: ShortUrl/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!loggedIn()) return View("Login");

            if (id == null)
            {
                return NotFound();
            }

            var shortUrl = await _context.ShortUrl
                .FirstOrDefaultAsync(m => m.id == id);
            if (shortUrl == null)
            {
                return NotFound();
            }

            return View(shortUrl);
        }

        // POST: ShortUrl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!loggedIn()) return View("Login");

            var shortUrl = await _context.ShortUrl.FindAsync(id);
            _context.ShortUrl.Remove(shortUrl);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login(string password)
        {
            if (password == "hardcoded")
            {
                Response.Cookies.Append("adamUrlLogin", "admin");
                ViewData["Login"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Login"] = false;
                ViewData["ErrorText"] = "Incorrect login";
                return View();
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("adamUrlLogin");
            return View("Login");
        }

        private bool ShortUrlExists(int id)
        {
            return _context.ShortUrl.Any(e => e.id == id);
        }
    }
}
