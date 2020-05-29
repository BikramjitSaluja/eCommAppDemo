using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.DataAccess.Data.Repository.IRepository;
using eCommerce.DataAccess.Models;
using eCommerceApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public ServiceViewModel SerViewModel { get; set; }

        public IActionResult Index(int? pageNumber)
        {
            ServiceViewModel serViewModel = new ServiceViewModel();
            serViewModel.PageNumber = (pageNumber == null ? 1 : Convert.ToInt32(pageNumber));
            serViewModel.PageSize = 3;

            // eager loading category and frequency class along with service
            List<Service> serList = _unitOfWork.ServiceRepository.GetAll(includeProperties: "Category,Frequency").ToList();

            if (serList.Count > 0)
            {
                serViewModel.ServiceList = serList.OrderBy(x => x.Id)
                         .Skip(serViewModel.PageSize * (serViewModel.PageNumber - 1))
                         .Take(serViewModel.PageSize).ToList();

                serViewModel.TotalCount = serList.Count;
                var page = (serViewModel.TotalCount / serViewModel.PageSize) - (serViewModel.TotalCount % serViewModel.PageSize == 0 ? 1 : 0);
                serViewModel.PagerCount = page + 1;
            }
            else
            {
                List<Service> emptyList = new List<Service>();
                serViewModel.ServiceList = emptyList;
                return View(serViewModel);
            }

            return View(serViewModel);
        }

        public IActionResult AddUpdate(int? id)
        {
            SerViewModel = new ServiceViewModel()
            {
                Service = new Service(),
                CategoryList = _unitOfWork.CategoryRepository.GetCategoryListForDropDown(),
                FrequencyList = _unitOfWork.FrequencyRepository.GetFrequencyListForDropDown()
            };

            if (id != null)
            {
                SerViewModel.Service = _unitOfWork.ServiceRepository.Get(id.GetValueOrDefault());
            }

            return View(SerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUpdate()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (SerViewModel.Service.Id == 0)
                {
                    //New Service
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    SerViewModel.Service.ImageUrl = @"\images\services\" + fileName + extension;

                    _unitOfWork.ServiceRepository.Add(SerViewModel.Service);
                }
                else
                {
                    //Edit Service
                    var serviceFromDb = _unitOfWork.ServiceRepository.Get(SerViewModel.Service.Id);
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\services");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }
                        SerViewModel.Service.ImageUrl = @"\images\services\" + fileName + extension_new;
                    }
                    else
                    {
                        SerViewModel.Service.ImageUrl = serviceFromDb.ImageUrl;
                    }

                    _unitOfWork.ServiceRepository.Update(SerViewModel.Service);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                SerViewModel.CategoryList = _unitOfWork.CategoryRepository.GetCategoryListForDropDown();
                SerViewModel.FrequencyList = _unitOfWork.FrequencyRepository.GetFrequencyListForDropDown();
                return View(SerViewModel);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var serviceFromDb = _unitOfWork.ServiceRepository.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (serviceFromDb == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _unitOfWork.ServiceRepository.Remove(serviceFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteConfirmation(int id)
        {
            return PartialView("_ConfirmDelete", id);
        }
    }
}