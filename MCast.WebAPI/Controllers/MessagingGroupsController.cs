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
using MCast.WebAPI.Services;
using Microsoft.Azure.NotificationHubs;

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

        [AllowAnonymous]
        [HttpGet("api/groups/{id}/messages")]
        public IActionResult GetMessagesForGroup(int id)
        {
            var messages = _context.GroupMessages
                .Where(m => m.MessagingGroupId == id)
                .OrderByDescending(m => m.Sent)
                .Take(20);

            return Ok(messages);
        }

        [AllowAnonymous]
        [HttpPost("api/groups/register")]
        public async Task<IActionResult> Register([FromBody]ClientData data)
        {
            var hub = NotificationService.Hub;

            var payload = @"<toast><visual><binding template=""ToastText01""><text id=""1"">$(message)</text></binding></visual></toast>";
            var registration = new WindowsTemplateRegistrationDescription(data.Handle, payload, new[] { $"group:{data.Group}" });
            registration.RegistrationId = await hub.CreateRegistrationIdAsync();
            var description = await hub.CreateOrUpdateRegistrationAsync(registration);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("api/groups/unregister")]
        public async Task<IActionResult> Unregister(ClientData data)
        {
            var hub = NotificationService.Hub;
            var regs =
                (await hub.GetRegistrationsByChannelAsync(data.Handle, 100));
                //.Where(r => r.Tags.Any(t => t == $"group:{data.Group}"));

            foreach (var r in regs)
            {
                await hub.DeleteRegistrationAsync(r);
            }

            return Ok();
        }

        [HttpPost("MessagingGroups/SendMessage/{id}")]
        public async Task <IActionResult> SendMessage(int id, [FromBody]SendMessageBodyWrapper body)
        {
            var props = new Dictionary<string, string> { { "message", body.Message } };
            await NotificationService.Hub.SendTemplateNotificationAsync(props, $"group:{id}");

            return Ok();
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

    public class ClientData
    {
        public string Handle { get; set; }
        public string Group { get; set; }
    }

    public class SendMessageBodyWrapper
    {
        public string Message { get; set; }
    }
}
