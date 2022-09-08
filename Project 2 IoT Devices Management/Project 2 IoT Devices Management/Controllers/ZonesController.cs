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
    public class ZonesController : Controller
    {
        private readonly Project2databaseContext _context;

        public ZonesController(Project2databaseContext context)
        {
            _context = context;
        }

        // GET: Zones
        [HttpGet("getAllZones")]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Zone.ToListAsync());
        }

        // GET: Zones/Details/5
        [HttpGet("getZone/{id}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zone = await _context.Zone
                .FirstOrDefaultAsync(m => m.ZoneId == id);
            if (zone == null)
            {
                return NotFound();
            }

            return Ok(zone);
        }


        [HttpPost("zoneAdd")]
        public async Task<IActionResult> Create(Zone zone)
        {
            zone.ZoneId = Guid.NewGuid();
            _context.Zone.Add(zone);
            await _context.SaveChangesAsync();
            return Ok("Added");
        }

       
        [HttpPatch("zoneEdit")]
        public async Task<IActionResult> Edit(Zone zone)
        {
            if (ZoneExists(zone.ZoneId))
            {
                try
                {
                    _context.Zone.Update(zone);
                    await _context.SaveChangesAsync();
                    return Ok("Edited");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZoneExists(zone.ZoneId))
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
            return NotFound("Not found");
        }

        

        // POST: Zones/Delete/5
        [HttpDelete("zoneDelete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var zone = await _context.Zone.FindAsync(id);
            _context.Zone.Remove(zone);
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }

        private bool ZoneExists(Guid id)
        {
            return _context.Zone.Any(e => e.ZoneId == id);
        }

        [HttpGet("getDevicesInZone/{id}")]
        public async Task<IActionResult> getDevicesInZone(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Device> zone = await _context.Device
                .Where(m => m.ZoneId == id).ToListAsync();
            if (zone == null)
            {
                return NotFound();
            }

            return Ok(zone);
        }
    }
}
