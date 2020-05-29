using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using eCommerceApp.ViewModels;
using eCommerce.DataAccess.Models;

namespace eCommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int? pageNumber)
        {
           
            CategoryViewModel catViewModel = new CategoryViewModel();
            catViewModel.PageNumber = (pageNumber == null ? 1 : Convert.ToInt32(pageNumber));
            catViewModel.PageSize = 3;

            List<Category> catList = _unitOfWork.CategoryRepository.GetAll().ToList();

            if(catList.Count > 0)
            {
                catViewModel.Categories = catList.OrderBy(x => x.Id)
                         .Skip(catViewModel.PageSize * (catViewModel.PageNumber - 1))
                         .Take(catViewModel.PageSize).ToList();

                catViewModel.TotalCount = catList.Count;
                var page = (catViewModel.TotalCount / catViewModel.PageSize) - (catViewModel.TotalCount % catViewModel.PageSize == 0 ? 1 : 0);
                catViewModel.PagerCount = page + 1;
            }
            else
            {
                List<Category> emptyList = new List<Category>();
                catViewModel.Categories = emptyList;
                return View(catViewModel);
            }

            return View(catViewModel);
        }

        public IActionResult AddUpdate(int? id)
        {
            Category category = new Category();
            if(id == null)
            {
                return View(category);
            }
            category = _unitOfWork.CategoryRepository.Get(id.GetValueOrDefault());
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUpdate(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    _unitOfWork.CategoryRepository.Add(category);
                }
                else
                {
                    _unitOfWork.CategoryRepository.Update(category);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.CategoryRepository.Get(id);
            if (objFromDb == null)
            {
               
                return RedirectToAction(nameof(Index));
            }

            _unitOfWork.CategoryRepository.Remove(objFromDb);
            _unitOfWork.Save();
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteConfirmation(int id)
        {
            return PartialView("_ConfirmDelete", id);
        }

    }
}