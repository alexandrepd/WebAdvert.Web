using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Api.Models;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.ServiceClients;
using WebAdvert.Web.Services;

namespace WebAdvert.Web.Controllers
{
    [Authorize]
    public class AdvertManagementController : Controller
    {
        private readonly IFileUploader _fileUploader;
        private readonly IAdvertApiClient _advertApiClient;
        private readonly IMapper _mapper;

        public AdvertManagementController(IFileUploader fileUploader, IAdvertApiClient advert, IMapper mapper)
        {
            _fileUploader = fileUploader;
            _advertApiClient = advert;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertNewModel createAdvertNewModel, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                AdvertModel _model = _mapper.Map<AdvertModel>(createAdvertNewModel);

                _model.UserName = User.Identity.Name;

                CreateAdvertResponse adverResponse = await _advertApiClient.Create(_model);
                string? id = string.Empty;

                if (adverResponse != null)
                    id = adverResponse.Id;
                else
                    throw new Exception(message: "Impossible to create a Advert.");


                string filePath = string.Empty;
                bool isOkToConfirm = true;

                if (formFile != null)
                {
                    var fileName = !string.IsNullOrEmpty(formFile.FileName) ? Path.GetFileName(formFile.FileName) : id;
                    filePath = $"{id}/{fileName}";

                    try
                    {
                        using (var readStream = formFile.OpenReadStream())
                        {
                            
                            bool result = await _fileUploader.UploadFileAsync(filePath, readStream);

                            if (!result)
                            {
                                throw new Exception(message: "Could not upload the image to file repository.");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        isOkToConfirm = false;
                        var confirmModel = new ConfirmAdvertModel { Id = id, Status = AdvertStatus.Pending };
                        await _advertApiClient.Confirm(confirmModel);
                        throw new Exception(message: "Error to read the file.");
                    }
                }

                if (isOkToConfirm)
                {
                    var confirmModel = new ConfirmAdvertModel { Id = id, FilePath = filePath, Status = AdvertStatus.Active };
                    await _advertApiClient.Confirm(confirmModel);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(createAdvertNewModel);
        }

        public async Task<IActionResult> Delete(string Id)
        {
            await _advertApiClient.Delete(Id);

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Edit(string Id)
        {
            AdvertModel _advertModel = await _advertApiClient.Get(Id);
            UpdateAdvertModel _model;
            if (_advertModel != null)
            {
                _model = new UpdateAdvertModel
                {
                    Id = Id,
                    Title = _advertModel.Title,
                    Description = _advertModel.Description,
                    Price = _advertModel.Price
                };
                return View(_model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAdvertModel model)
        {
            if (ModelState.IsValid)
            {
                AdvertModel _advertModel = await _advertApiClient.Get(model.Id);
                _advertModel.Title = model.Title;
                _advertModel.Description = model.Description;
                _advertModel.Price = model.Price;

                await _advertApiClient.Update(_advertModel);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
