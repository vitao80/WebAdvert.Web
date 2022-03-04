using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.Web.Models;
using WebAdvert.Web.Services;

namespace WebAdvert.Web.Controllers
{
    public class AdvertsUploadController : Controller
    {
        readonly IFileUploader _fileUploader;

        public AdvertsUploadController(IFileUploader fileUploader)
        {
            _fileUploader = fileUploader;
        }

        public IActionResult Create()
        {
            return View(new CreateAdvertViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertViewModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                //TODO: criar o objeto com chamada http da api
                var id = "11111";
                var filename = "";

                if (imageFile != null)
                {
                    filename = !string.IsNullOrEmpty(imageFile.FileName) ? Path.GetFileName(imageFile.FileName) : id;
                    var filePath = $"{id}/{filename}";
                    try
                    {
                        using (var readStream = imageFile.OpenReadStream())
                        {
                            var result = await _fileUploader.UploadFileAsync(filePath, readStream).ConfigureAwait(false);
                            if (!result)
                                throw new Exception("Não foi possível subir o arquivo.");

                        }
                        //TODO: chamar advertApi e confirmar o registro

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception e)
                    {
                        //TODO: call advertApi para cancelar o registro
                        Console.WriteLine(e);
                        
                    }
                }

            }
            return View();
        }
    }
}
