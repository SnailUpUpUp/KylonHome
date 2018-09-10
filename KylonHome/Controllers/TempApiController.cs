using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KylonHome.Data;
using KylonHome.Models;

namespace KylonHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TempApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/TempApi
        [HttpPost]
        public async Task<IActionResult> PostTemperature([FromBody] Temperature temperature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Temperature.Add(temperature);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTemperature", new { id = temperature.ID }, temperature);
        }

        //// GET: api/TempApi
        //[HttpGet]
        //public IEnumerable<Temperature> GetTemperature()
        //{
        //    return _context.Temperature;
        //}

        // GET: api/TempApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTemperature([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var temperature = await _context.Temperature.FindAsync(id);

            if (temperature == null)
            {
                return NotFound();
            }

            return Ok(temperature);
        }

        //// PUT: api/TempApi/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTemperature([FromRoute] int id, [FromBody] Temperature temperature)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != temperature.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(temperature).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TemperatureExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        //// DELETE: api/TempApi/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTemperature([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var temperature = await _context.Temperature.FindAsync(id);
        //    if (temperature == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Temperature.Remove(temperature);
        //    await _context.SaveChangesAsync();

        //    return Ok(temperature);
        //}

        private bool TemperatureExists(int id)
        {
            return _context.Temperature.Any(e => e.ID == id);
        }
    }
}