namespace SportsStore.Models.ViewModels
{
    public class ProductListViewModel
    {
        public IList<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
