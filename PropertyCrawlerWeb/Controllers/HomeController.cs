using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PropertyCrawler.Data.Repositories;
using PropertyCrawlerWeb.Models;

namespace PropertyCrawlerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IProcessRepository _repo;

        public HomeController(UserManager<IdentityUser> userManager, IProcessRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {

                return LocalRedirect("/Identity/Account/Login");
            }
            var model = _repo.GeneralProcessInfo();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
