using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PropertyCrawler.Data;
using PropertyCrawler.Data.Entity;
using AppContext = PropertyCrawler.Data.AppContext;

namespace PropertyCrawlerWeb.Controllers
{
    public class ProxyIpsController : Controller
    {
        private readonly AppContext _context;

        public ProxyIpsController(AppContext context)
        {
            _context = context;
        }

        // GET: ProxyIps
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProxyIps.ToListAsync());
        }

        // GET: ProxyIps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proxyIp = await _context.ProxyIps
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proxyIp == null)
            {
                return NotFound();
            }

            return View(proxyIp);
        }

        // GET: ProxyIps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProxyIps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ip,Port,Username,Password,Id,DateAdded,DateModified,Active")] ProxyIp proxyIp)
        {
            if (ModelState.IsValid)
            {
                proxyIp.DateAdded = DateTime.Now;
                proxyIp.DateModified = DateTime.Now;
                _context.Add(proxyIp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proxyIp);
        }

        // GET: ProxyIps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proxyIp = await _context.ProxyIps.FindAsync(id);
            if (proxyIp == null)
            {
                return NotFound();
            }
            return View(proxyIp);
        }

        // POST: ProxyIps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Ip,Port,Username,Password,Id,DateAdded,DateModified,Active")] ProxyIp proxyIp)
        {
            if (id != proxyIp.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    proxyIp.DateModified = DateTime.Now;
                    _context.Update(proxyIp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProxyIpExists(proxyIp.Id))
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
            return View(proxyIp);
        }

        // GET: ProxyIps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proxyIp = await _context.ProxyIps
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proxyIp == null)
            {
                return NotFound();
            }

            return View(proxyIp);
        }

        // POST: ProxyIps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proxyIp = await _context.ProxyIps.FindAsync(id);
            _context.ProxyIps.Remove(proxyIp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProxyIpExists(int id)
        {
            return _context.ProxyIps.Any(e => e.Id == id);
        }
    }
}
