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
        public ProcessController(IPostalCodeRepository repoPostalCode, IProcessRepository repo, IJobService jobService)
        {
            _repoPostalCode = repoPostalCode;
            _repo = repo;
            _jobService = jobService;
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

        public async Task<IActionResult> List(string param, int? pageNumber)
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
        public async Task<IActionResult> Execute(List<string> postalCodes, PropertyType propertyType, ProcessType processType, bool isScheduled, ScheduleInterval? scheduleInterval)
        {
            if(isScheduled==true && scheduleInterval==null)
            {
                return BadRequest("Model is not valid");
            }
            
            var lista = await _repoPostalCode.GetPostalCodesAsync(postalCodes);
            //var process = new Process
            //{
            //    DateAdded = DateTime.UtcNow,
            //    Active = true,
            //    Status = ProcessStatus.Failed,
            //    Type = processType,
            //    PropertyType = propertyType,
            //    DateModified = DateTime.UtcNow
            //};
            //foreach (var item in process.ProcessPostalCodes)
            //{
            //    _context.ProcessPostalCode.Add(new ProcessPostalCode
            //    {
            //        ProcessId = process.Id,
            //        PostalCodeId = item.PostalCodeId
            //    });
            //}


            await _jobService.Job(lista, propertyType, processType, isScheduled, scheduleInterval);

            //do thirret service
            return View();
        }
    }
}