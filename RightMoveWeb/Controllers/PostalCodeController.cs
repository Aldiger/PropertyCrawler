using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PropertyCrawler.Data.Models;
using PropertyCrawler.Data.Repositories;
using PropertyCrawlerWeb.Models;

namespace PropertyCrawlerWeb.Controllers
{

    public class PostalCodeController : Controller
    {
        public const int pagesize = 30;
        private readonly IPostalCodeRepository _repo;

        public PostalCodeController(IPostalCodeRepository repo)
        {
            _repo = repo;
        }


        public IActionResult Index(int? pageNumber, string sort, List<string> postal_code)
        {
            if (!pageNumber.HasValue || pageNumber < 1)
            {
                pageNumber = 1;
            }
            //if (!string.IsNullOrEmpty(sort))
            //    ViewData["CurrentSort"] = sort;
            //else
            //    ViewData["CurrentSort"] = "";

            var query = _repo.GetAllPostalCodesAsync(sort, postal_code);
            var totalCount = query.Count();
            var items = query
                .Skip((pageNumber.Value - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            var pgmodel = new PagedList<PostalCodeModel>
            {
                CurrentPage = pageNumber.Value,
                Items = items,
                PageSize = pagesize,
                TotalCount = totalCount
            };

            return View(pgmodel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostalCodeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model State is not valid");
            }

            await _repo.CreatePostalCodeAsync(model);

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item = await _repo.GetById(id);

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PostalCodeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model State is not valid");
            }

            await _repo.UpdatePostalCodeAsync(model);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeletePostalCodeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var lista = await _repo.AllPostalCodesSelect();

            ViewBag.Postal = new SelectList(lista, "Text");
            return View();
        }

    }
}