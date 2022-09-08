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
    public class CategoriesController : Controller
    {
        private readonly Project2databaseContext _context;

        public CategoriesController(Project2databaseContext context)
        {
            _context = context;
        }

        // GET: Categories
        [HttpGet("getCategoriesAll")]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Category.ToListAsync());
        }

        // GET: Categories/Details/5
        [HttpGet("getCategory/{id}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("getCategoryDevices/{id}")]
        public async Task<IActionResult> categoryDevices(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Device> category = await _context.Device
                .Where(m => m.CategoryId == id).ToListAsync();
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("getCategoryZones/{id}")]
        public async Task<IActionResult> zonesInCategory(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int zones = await _context.Device
                .FromSqlRaw("Select Device.DeviceId, Device.CategoryId from Device join Zone on Device.ZoneId=Zone.ZoneId").Where(m => m.CategoryId == id).CountAsync();

            return Ok(zones);
        }

        [HttpPost("categoryAdd")]
        public async Task<IActionResult> Create(Category category)
        {
            category.CategoryId = Guid.NewGuid();
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return Ok("Added"); ;
        }

        [HttpPatch("categoryEdit")]
        public async Task<IActionResult> Edit(Category category)
        {
            if (CategoryExists(category.CategoryId))
            {
                try
                {
                    _context.Category.Update(category);
                    await _context.SaveChangesAsync();
                    return Ok("Edited");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return Ok("Not valid");
        }

        // POST: Categories/Delete/5
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }
    }
}
