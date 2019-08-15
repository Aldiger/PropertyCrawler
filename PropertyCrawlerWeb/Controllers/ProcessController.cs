using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PropertyCrawler.Data;
using PropertyCrawler.Data.Entity;
using PropertyCrawler.Data.Models;
using PropertyCrawler.Data.Repositories;
using PropertyCrawlerWeb.Helpers;
using PropertyCrawlerWeb.Models;
using PropertyCrawlerWeb.Services;

namespace PropertyCrawlerWeb.Controllers
{

    public class ProcessController : Controller
    {
        public const int pagesize = 30;
        private readonly IPostalCodeRepository _repoPostalCode;
        private readonly IProcessRepository _repo;
        private readonly IJobService _jobService;
        private readonly PropertyCrawler.Data.AppContext _context;
        public ProcessController(IPostalCodeRepository repoPostalCode, IProcessRepository repo, IJobService jobService, PropertyCrawler.Data.AppContext contexte)
        {
            _repoPostalCode = repoPostalCode;
            _repo = repo;
            _jobService = jobService;
            _context = contexte;
        }

        [HttpGet]
        public async Task<IActionResult> Configure()
        {
            var lista = await _repoPostalCode.AllPostalCodesSelect();
            var proxyIps = _context.ProxyIps.Where(x=>x.Active).Select(x=>String.IsNullOrWhiteSpace(x.Port) ? x.Ip: x.Ip+":"+x.Port).ToList();
            ViewBag.ProxyIps = new SelectList(proxyIps, "Text");
            
            ViewBag.Postal = new SelectList(lista, "Text");
            ViewBag.PropertyType = new SelectList(EnumHelper.PropertyTypeList(), "Id", "Text");
            ViewBag.ProcessType = new SelectList(EnumHelper.ProcessTypeList(), "Id", "Text");
            ViewBag.ScheduleIntervalType = new SelectList(EnumHelper.ScheduleIntervalType(), "Id", "Text");

            return View();
        }



        public async Task<IActionResult> Index(string param, int? pageNumber)
        {

            if (!pageNumber.HasValue || pageNumber < 1)
            {
                pageNumber = 1;
            }

            var query = await _repo.GetProcessesByStatus(param);
            var totalCount = query.Count();
            var items = query
                .Skip((pageNumber.Value - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            var pgmodel = new PagedList<ProcessVM>
            {
                CurrentPage = pageNumber.Value,
                Items = items,
                PageSize = pagesize,
                TotalCount = totalCount
            };

            return View(pgmodel);
        }


        [HttpPost]
        public async Task<IActionResult> Execute(List<string> postalCodes, PropertyType propertyType, ProcessType processType, bool isScheduled, ScheduleInterval? scheduleInterval, string proxyIp, string portal)
        {
            if (isScheduled == true && scheduleInterval == null)
            {
                return BadRequest("Model is not valid");
            }

            var listPostalCodes = await _repoPostalCode.GetPostalCodesAsync(postalCodes);
            var proxy = _context.ProxyIps.FirstOrDefault(x => x.Ip == proxyIp);

            //do thirret service
            await _jobService.Job(listPostalCodes, propertyType, processType, isScheduled, scheduleInterval, proxy);


            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> ReExecute(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var model = await _repo.GetProcessById((int)id);

            var postalCodes = model.ProcessPostalCodes.Select(x => x.PostalCode).ToList();
            PropertyType propertyType = model.PropertyType;
            ProcessType processType = model.Type;
            bool isScheduled = false;

            await _jobService.Job(postalCodes, propertyType, processType, isScheduled, scheduleInterval: null,proxyIp: null);

            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Details(int? id)
        {
            if(!id.HasValue)
            {
                return NotFound();
            }

            var model = await _repo.GetProcessVmById((int)id);

            return View(model);

        }

    }
}