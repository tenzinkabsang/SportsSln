using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly StoreDbContext _ctx;
        public OrderRepository(StoreDbContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<Order> Orders => _ctx.Orders
            .Include(o => o.Lines)
            .ThenInclude(o => o.Product);

        public void SaveOrder(Order order)
        {
            List<Product> items = order.Lines.Select(l => l.Product).ToList();
            _ctx.AttachRange(items);
            if(order.OrderID == 0)
            {
                _ctx.Orders.Add(order);
            }
            _ctx.SaveChanges();
        }
    }
}
