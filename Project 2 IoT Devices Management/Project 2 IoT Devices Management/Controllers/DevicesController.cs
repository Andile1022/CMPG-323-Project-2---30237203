using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_2_IoT_Devices_Management.Model;

namespace Project_2_IoT_Devices_Management.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class DevicesController : Controller
    {
        private readonly Project2databaseContext _context;

        public DevicesController(Project2databaseContext context)
        {
            _context = context;
        }

        // GET: Devices
        [HttpGet("getDevicesAll")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Device.ToListAsync());
        }

        // GET: Devices/Details/5
        [HttpGet("getDevice/{id}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Device
                .FirstOrDefaultAsync(m => m.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

      

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("deviceAdd")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActvie,DateCreated")] Device device)
        {
            if (ModelState.IsValid)
            {
                device.DeviceId = Guid.NewGuid();
                _context.Add(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }

       
        [HttpPatch]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActvie,DateCreated")] Device device)
        {
            if (id != device.DeviceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.DeviceId))
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
            return View(device);
        }

        // GET: Devices/Delete/5
        [HttpDelete("deviceDelete")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Device
                .FirstOrDefaultAsync(m => m.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var device = await _context.Device.FindAsync(id);
            _context.Device.Remove(device);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(Guid id)
        {
            return _context.Device.Any(e => e.DeviceId == id);
        }
    }
}
