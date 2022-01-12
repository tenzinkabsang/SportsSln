using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository _storeRepository;


        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }

        public CartModel(IStoreRepository storeRepository, Cart cart)
        {
            _storeRepository = storeRepository;
            Cart = cart;
        }


        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long productId, string returnUrl)
        {
            Product product = _storeRepository.Products.FirstOrDefault(p => p.ProductID == productId);
            Cart.AddItem(product, quantity: 1);
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(long productId, string returnUrl)
        {
            var product = Cart.Lines.First(cl => cl.Product.ProductID == productId).Product;
            Cart.RemoveLine(product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
