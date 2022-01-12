using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository _orderRepository;
        private Cart _cart;

        public OrderController(IOrderRepository orderRepository, Cart cart)
        {
            _orderRepository = orderRepository;
            _cart = cart;
        }
        public IActionResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (_cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Sorry, your cart is empty!");

            if (!ModelState.IsValid)
                return View();

            order.Lines = _cart.Lines.ToArray();
            _orderRepository.SaveOrder(order);
            _cart.Clear();
            return RedirectToPage("/Completed", new { orderId = order.OrderID });
        }
    }
}
