using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAdvert.Api.Models;
using WebAdvert.Web.Models;
using WebAdvert.Web.ServiceClients;

namespace WebAdvert.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdvertApiClient _advertApiClient;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IAdvertApiClient advertApiClient, IMapper mapper)
        {
            _logger = logger;
            _advertApiClient = advertApiClient;
            _mapper = mapper;

        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<AdvertModel> advertModels = await _advertApiClient.GetAll();
            return View(advertModels);
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