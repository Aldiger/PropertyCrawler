using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RightMove.Data.Repositories;

namespace RightMoveWeb.Controllers
{
    public class ProcessController : Controller
    {
        private readonly IProcessRepository _repo;

        public ProcessController(IProcessRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Lista e proceseve
            return View();
        }


        public IActionResult Details()
        {
            // Details of a process
            return View();
        }



        public async Task<IActionResult> Start()
        {

            var lista = await _repo.GetAllPostalCodes();

            ViewBag.Postal = new SelectList(lista, "Text");

            // Start process
            return View();
        }


    }
}