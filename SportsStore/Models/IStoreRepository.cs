namespace SportsStore.Models
{
    // Exposing IQueryable allows clients to poke into the database anytime they want. Could be hard to track down but it is faster and less code
    // Caching could be a problem.
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }
    }

    public class EFStoreRepository : IStoreRepository
    {
        private StoreDbContext _dbContext;
        public EFStoreRepository(StoreDbContext ctx)
        {
            _dbContext = ctx;
        }

        public IQueryable<Product> Products => _dbContext.Products;
    }
}
