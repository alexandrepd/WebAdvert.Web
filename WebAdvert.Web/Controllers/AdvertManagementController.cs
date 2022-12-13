using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.Services;

namespace WebAdvert.Web.Controllers
{
    public class AdvertManagementController : Controller
    {
        private readonly IFileUploader _fileUploader;

        public AdvertManagementController(IFileUploader fileUploader)
        {
            _fileUploader = fileUploader;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            CreateAdvertNewModel createAdvertNewModel = new CreateAdvertNewModel();
            return View(createAdvertNewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertNewModel createAdvertNewModel, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    try
                    {
                        using (var readStream = formFile.OpenReadStream())
                        {
                            bool result = await _fileUploader.UploadFileAsync(formFile.FileName, readStream);

                            if (!result)
                            {
                                throw new Exception(message: "Could not upload the image to file repository.");
                            }
                        }
                    }
                    catch (Exception)
                    {

                        throw new Exception(message: "Error to read the file.");
                    }

                }

            }
            return View("Home");
        }
    }
}
