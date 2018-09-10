using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KylonHome.Data;
using KylonHome.Models;
using Microsoft.AspNetCore.Authorization;

namespace KylonHome.Controllers
{
    public class TemperaturesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TemperaturesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Temperatures
        public async Task<IActionResult> Index(string DeviceName, string Begin, string End)
        {
            ViewData["Title"] = "溫濕度數據查詢";

            DateTime begin, end;
            if (!string.IsNullOrEmpty(Begin))
            {
                ViewBag.Begin = Begin;
                begin = Convert.ToDateTime(Begin);
            }
            else
            {
                var AcqDates = from t in _context.Temperature
                               where DateTime.Compare(DateTime.Now.AddHours(-1),t.AcquisitionTime)<=0
                               select t.AcquisitionTime;
                string maxAcqDate;
                if (AcqDates.Count() > 0)
                    maxAcqDate = DateTime.Now.AddHours(-1).ToString("yyyy/MM/dd HH:mm:ss");
                else
                    maxAcqDate = (from t in _context.Temperature
                                  select t.AcquisitionTime).Max().ToString("yyyy/MM/dd HH:mm:ss");
                if (!string.IsNullOrEmpty(maxAcqDate))
                {
                    ViewBag.Begin = maxAcqDate;
                    begin = Convert.ToDateTime(maxAcqDate);
                }
                else
                {
                    ViewBag.Begin = DateTime.Now.AddHours(-1).ToString("yyyy/MM/dd HH:mm:ss");
                    begin = Convert.ToDateTime(DateTime.Now.AddHours(-1).ToString("yyyy/MM/dd HH:mm:ss"));
                }
            }

            if (!string.IsNullOrEmpty(End))
            {
                ViewBag.End = End;
                end = Convert.ToDateTime(End);
            }
            else
            {
                ViewBag.End = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                end = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            }

            var temps = from t in _context.Temperature
                        where DateTime.Compare(begin,t.AcquisitionTime)<=0 && DateTime.Compare(t.AcquisitionTime,end)<=0
                        select t;
            if (!string.IsNullOrEmpty(DeviceName))
                temps = temps.Where(t => t.DeviceName.Contains(DeviceName));

            temps = temps.OrderByDescending(t => t.AcquisitionTime);

            return View(await temps.ToListAsync());
            // return View(await _context.Temperature.ToListAsync());
        }

        // GET: Temperatures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temperature = await _context.Temperature
                .FirstOrDefaultAsync(m => m.ID == id);
            if (temperature == null)
            {
                return NotFound();
            }

            return View(temperature);
        }

        // GET: Temperatures/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Temperatures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,DeviceName,Temp,Humidity,AcquisitionTime,Memo")] Temperature temperature)
        {
            if (ModelState.IsValid)
            {
                _context.Add(temperature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(temperature);
        }

        // GET: Temperatures/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temperature = await _context.Temperature.FindAsync(id);
            if (temperature == null)
            {
                return NotFound();
            }
            return View(temperature);
        }

        // POST: Temperatures/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DeviceName,Temp,Humidity,AcquisitionTime,Memo")] Temperature temperature)
        {
            if (id != temperature.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(temperature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemperatureExists(temperature.ID))
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
            return View(temperature);
        }

        // GET: Temperatures/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temperature = await _context.Temperature
                .FirstOrDefaultAsync(m => m.ID == id);
            if (temperature == null)
            {
                return NotFound();
            }

            return View(temperature);
        }

        // POST: Temperatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var temperature = await _context.Temperature.FindAsync(id);
            _context.Temperature.Remove(temperature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TemperatureExists(int id)
        {
            return _context.Temperature.Any(e => e.ID == id);
        }
    }
}
