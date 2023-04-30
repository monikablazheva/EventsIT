using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventsIT.Data;
using EventsIT.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace EventsIT.Controllers
{
    [Authorize(Roles = "Basic")]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TicketsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public User GetCustomer()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.Users.Include(u => u.Tickets).FirstOrDefault(u => u.Id == currentUserId);
            return user;
        }
        // GET: Tickets
        public async Task<IActionResult> Index()
        {
              return _context.Tickets.Include(t=>t.User).Include(t=>t.Event).Where(t=>t.User==GetCustomer()) != null ? 
                          View(await _context.Tickets.Include(t => t.User).Include(t => t.Event).Where(t => t.User == GetCustomer()).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.Include(t => t.User).Include(t => t.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? eventId)
        {
            if (eventId == null || _context.Events == null)
            {
                return NotFound();
            }

            var eventFromDb = await _context.Events.FindAsync(eventId);
            if (eventFromDb == null)
            {
                return NotFound();
            }


            Ticket ticket = new Ticket();
            ticket.User = GetCustomer();
            ticket.Event = eventFromDb;

            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            return RedirectToAction("Deatils", new { id = ticket.Id });
        }


        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.Include(t => t.User).Include(t => t.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
