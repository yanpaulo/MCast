using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MCast.WebAPI.Data;
using MCast.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MCast.WebAPI.Controllers
{
    [Authorize]
    public class MessagingGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagingGroupsController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: MessagingGroups
        public IActionResult Index()
        {
            return View(GetUser().MessagingGroups.ToList());
        }

        // GET: MessagingGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messagingGroup = await _context.MessagingGroups
                .SingleOrDefaultAsync(m => m.Id == id);
            if (messagingGroup == null)
            {
                return NotFound();
            }

            return View(messagingGroup);
        }

        // GET: MessagingGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MessagingGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ApplicationUserId")] MessagingGroup messagingGroup)
        {
            if (ModelState.IsValid)
            {
                var user = GetUser();
                user.MessagingGroups.Add(messagingGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(messagingGroup);
        }

        // GET: MessagingGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = GetUser();
            if (!UserHasGroup(user, id.Value))
            {
                return Unauthorized();
            }

            var messagingGroup = await _context.MessagingGroups.SingleOrDefaultAsync(m => m.Id == id);
            if (messagingGroup == null)
            {
                return NotFound();
            }
            return View(messagingGroup);
        }

        // POST: MessagingGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ApplicationUserId")] MessagingGroup messagingGroup)
        {
            if (id != messagingGroup.Id)
            {
                return NotFound();
            }
            var user = GetUser();
            if (!UserHasGroup(user, messagingGroup.Id))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                
                try
                {
                    var group = user.MessagingGroups.Single(g => g.Id == messagingGroup.Id);
                    group.Name = messagingGroup.Name;
                    await _context.SaveChangesAsync();
                }
                catch (InvalidOperationException)
                {
                    if (!MessagingGroupExists(messagingGroup.Id))
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
            return View(messagingGroup);
        }

        // GET: MessagingGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = GetUser();
            if (!UserHasGroup(user, id.Value))
            {
                return Unauthorized();
            }

            var messagingGroup = await _context.MessagingGroups
                .SingleOrDefaultAsync(m => m.Id == id);
            if (messagingGroup == null)
            {
                return NotFound();
            }

            return View(messagingGroup);
        }

        // POST: MessagingGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = GetUser();
            if (!UserHasGroup(user, id))
            {
                return Unauthorized();
            }

            var messagingGroup = await _context.MessagingGroups.SingleOrDefaultAsync(m => m.Id == id);
            _context.MessagingGroups.Remove(messagingGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessagingGroupExists(int id)
        {
            return _context.MessagingGroups.Any(e => e.Id == id);
        }

        private ApplicationUser GetUser()
        {
            var name = User.FindFirst(ClaimTypes.Name).Value;
            return _context.Users.Include(u => u.MessagingGroups).Single(u => u.UserName == name);
        }

        private bool UserHasGroup(ApplicationUser user, int groupId)
        {
            return user.MessagingGroups.Any(g => g.Id == groupId);
        }
    }
}
