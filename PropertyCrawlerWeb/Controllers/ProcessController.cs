using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PropertyCrawler.Data.Repositories;
using PropertyCrawlerWeb.Helpers;

namespace PropertyCrawlerWeb.Controllers
{
    public class ProcessController : Controller
    {
        private readonly IPostalCodeRepository _repoPostalCode;
        public ProcessController(IPostalCodeRepository repoPostalCode)
        {
            _repoPostalCode = repoPostalCode;
        }

        [HttpGet]
        public async Task<IActionResult> Configure()
        {
            var lista = await _repoPostalCode.AllPostalCodesSelect();
            ViewBag.Postal = new SelectList(lista, "Text");
            ViewBag.PropertyType = new SelectList(EnumHelper.PropertyTypeList(), "Id", "Text");
            ViewBag.ProcessType = new SelectList(EnumHelper.ProcessTypeList(), "Id", "Text");
            ViewBag.ScheduleIntervalType = new SelectList(EnumHelper.ScheduleIntervalType(), "Id", "Text");

            return View();


        }
    }
}