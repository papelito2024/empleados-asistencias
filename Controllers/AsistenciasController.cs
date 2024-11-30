using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using test2.Data;

namespace test2.Controllers
{
    
    public class AsistenciasController : Controller
    {
        private readonly ILogger<AsistenciasController> _logger;
        private readonly ApplicationDbContext _context;
        public AsistenciasController(ApplicationDbContext context,ILogger<AsistenciasController> logger)
        {

            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}