using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BrsBlogWeb.Data;
using BrsBlogWeb.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BrsBlogWeb.Controllers
{
    public class AdminsController : Controller
    {
        private readonly BrsAdminsContext _context;

        public AdminsController(BrsAdminsContext context)
        {
            _context = context;
        }
        
        // GET: Admins
        public async Task<IActionResult> Index()
        {
            return View(await _context.Admins.ToListAsync());
        }
        
        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admins = await _context.Admins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admins == null)
            {
                return NotFound();
            }

            return View(admins);
        }
        
        // GET: Admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AdminName,AdminEmail,AdminPassword")] Admins admins)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admins);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admins);
        }
        
        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admins = await _context.Admins.FindAsync(id);
            if (admins == null)
            {
                return NotFound();
            }
            return View(admins);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AdminName,AdminEmail,AdminPassword")] Admins admins)
        {
            if (id != admins.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admins);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminsExists(admins.Id))
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
            return View(admins);
        }
       
        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admins = await _context.Admins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admins == null)
            {
                return NotFound();
            }

            return View(admins);
        }
        
        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admins = await _context.Admins.FindAsync(id);
            _context.Admins.Remove(admins);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AdminLogin(Admins admin)
        {
            var datavalue = _context.Admins.FirstOrDefault(x => x.AdminName == admin.AdminName && x.AdminEmail == admin.AdminEmail
            &&x.AdminPassword==admin.AdminPassword);
            Console.WriteLine(admin.AdminName+admin.AdminEmail+admin.AdminPassword);
            if (datavalue != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.AdminName),
                };
                var useridentity = new ClaimsIdentity(claims,"Login");  
                ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index","BlogPosts");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AdminLogin", "Admins");
        }
        private bool AdminsExists(int id)
        {
            return _context.Admins.Any(e => e.Id == id);
        }
    }
}
