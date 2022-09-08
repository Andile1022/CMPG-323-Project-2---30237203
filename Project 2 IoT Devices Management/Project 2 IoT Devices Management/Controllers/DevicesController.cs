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
            return Ok(await _context.Device.ToListAsync());
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

            return Ok(device);
        }

      

        [HttpPost("deviceAdd")]
        public async Task<IActionResult> Create(Device device)
        {
            device.DeviceId = Guid.NewGuid();
            _context.Device.Add(device);
            await _context.SaveChangesAsync();
            return Ok("Added");
        }

       
        [HttpPatch("DeviceEdit")]
        public async Task<IActionResult> Edit(Device device)
        {

            if (DeviceExists(device.DeviceId))
            {
                try
                {
                    _context.Device.Update(device);
                    await _context.SaveChangesAsync();
                    return Ok("Edited");
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
                return Ok("Error");
            }
            return Ok(device);
        }

        // POST: Devices/Delete/5
        [HttpPost("deleteDevice")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var device = await _context.Device.FindAsync(id);
            _context.Device.Remove(device);
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }

        private bool DeviceExists(Guid id)
        {
            return _context.Device.Any(e => e.DeviceId == id);
        }
    }
}
