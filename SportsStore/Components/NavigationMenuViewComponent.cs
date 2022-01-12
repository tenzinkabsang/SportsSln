using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IStoreRepository _repository;

        public NavigationMenuViewComponent(IStoreRepository repo)
        {
            _repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _repository.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(p => p)
                .ToList();

            ViewBag.SelectedCategory = RouteData?.Values["category"];

            return View(categories);
        }
    }
}
