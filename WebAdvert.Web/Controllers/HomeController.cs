using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAdvert.Api.Models;
using WebAdvert.Web.Models;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.Models.Home;
using WebAdvert.Web.ServiceClients;

namespace WebAdvert.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdvertApiClient _advertApiClient;
        private readonly IMapper _mapper;
        private readonly ISearchApiClient _searchApiClient;

        public HomeController(ILogger<HomeController> logger, IAdvertApiClient advertApiClient, IMapper mapper, ISearchApiClient searchApiClient)
        {
            _logger = logger;
            _advertApiClient = advertApiClient;
            _mapper = mapper;
            _searchApiClient = searchApiClient;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<AdvertModel> advertModels = await _advertApiClient.GetAll();
            List<AdvertCardModel> list = advertModels.Select(advertModel =>

            new AdvertCardModel()
            {
                Id = advertModel.Id,
                Title = advertModel.Title,
                Description = advertModel.Description,
                Image = advertModel.FilePath

            }).ToList();
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string keyword)
        {
            var viewModel = new List<SearchViewModel>();
            var searchResult = await _searchApiClient.Search(keyword);

            foreach (AdvertType doc in searchResult)
            {
                var viewModelItem = _mapper.Map<SearchViewModel>(doc);
                viewModel.Add(viewModelItem);
            }

            return View("../AdvertManagement/SearchCard", viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}