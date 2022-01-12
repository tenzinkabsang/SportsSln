using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository _repository;
        public int PageSize = 4;

        public HomeController(IStoreRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index(string category, int productPage = 1)
        {

            var products = _repository.Products.Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductID)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();


            var viewModel = new ProductListViewModel
            {
                Products = products,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? _repository.Products.Count() : _repository.Products.Count(p => p.Category == category)

                },
                CurrentCategory = category
            };
            return View(viewModel);
        }
    }
}
