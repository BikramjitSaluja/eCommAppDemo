using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.DataAccess.Data.Repository.IRepository;
using eCommerce.DataAccess.Models;
using eCommerceApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int? pageNumber)
        {

            FrequencyViewModel freqViewModel = new FrequencyViewModel();
            freqViewModel.PageNumber = (pageNumber == null ? 1 : Convert.ToInt32(pageNumber));
            freqViewModel.PageSize = 3;

            List<Frequency> freqList = _unitOfWork.FrequencyRepository.GetAll().ToList();

            if (freqList.Count > 0)
            {
                freqViewModel.Frequencies = freqList.OrderBy(x => x.Id)
                         .Skip(freqViewModel.PageSize * (freqViewModel.PageNumber - 1))
                         .Take(freqViewModel.PageSize).ToList();

                freqViewModel.TotalCount = freqList.Count;
                var page = (freqViewModel.TotalCount / freqViewModel.PageSize) - (freqViewModel.TotalCount % freqViewModel.PageSize == 0 ? 1 : 0);
                freqViewModel.PagerCount = page + 1;
            }
            else
            {
                List<Frequency> emptyList = new List<Frequency>();
                freqViewModel.Frequencies = emptyList;
                return View(freqViewModel);
            }

            return View(freqViewModel);
        }

        public IActionResult AddUpdate(int? id)
        {
            Frequency frequency = new Frequency();
            if (id == null)
            {
                return View(frequency);
            }
            frequency = _unitOfWork.FrequencyRepository.Get(id.GetValueOrDefault());
            if (frequency == null)
            {
                return NotFound();
            }
            return View(frequency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUpdate(Frequency frequency)
        {
            if (ModelState.IsValid)
            {
                if (frequency.Id == 0)
                {
                    _unitOfWork.FrequencyRepository.Add(frequency);
                }
                else
                {
                    _unitOfWork.FrequencyRepository.Update(frequency);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(frequency);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.FrequencyRepository.Get(id);
            if (objFromDb == null)
            {

                return RedirectToAction(nameof(Index));
            }

            _unitOfWork.FrequencyRepository.Remove(objFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteConfirmation(int id)
        {
            return PartialView("_ConfirmDelete", id);
        }
    }
}