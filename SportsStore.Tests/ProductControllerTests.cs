using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests;

public class ProductControllerTests
{
    [Fact]
    public void Can_Use_Repository()
    {
        // Arrange
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns((
            new Product[]
            {
                new Product{ProductID = 1, Name = "P1"},
                new Product{ProductID = 2, Name = "P2"}
            }).AsQueryable());

        var controller = new HomeController(mock.Object);

        // Act
        var result = controller.Index(null).GetViewData<ProductListViewModel>();

        // Assert
        Product[] products = result.Products.ToArray();
        Assert.True(products.Length == 2);
        Assert.Equal("P1", products[0].Name);
        Assert.Equal("P2", products[1].Name);
    }

    [Fact]
    public  void Can_Paginate()
    {
        // Arrange
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns((
             new Product[]
            {
                new Product{ProductID = 1, Name = "P1"},
                new Product{ProductID = 2, Name = "P2"},
                new Product{ProductID = 3, Name = "P3"},
                new Product{ProductID = 4, Name = "P4"},
                new Product{ProductID = 5, Name = "P5"}
            }).AsQueryable());

        var controller = new HomeController(mock.Object);
        controller.PageSize = 3;

        // Act
        var result = controller.Index(null, 2).GetViewData<ProductListViewModel>();

        // Assert
        Product[] products = result.Products.ToArray();
        Assert.True(products.Length == 2);
        Assert.Equal("P4", products[0].Name);
        Assert.Equal("P5", products[1].Name);
    }

    [Fact]
    public void Can_Send_Pagination_View_Model()
    {
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns((
             new Product[]
            {
                new Product{ProductID = 1, Name = "P1"},
                new Product{ProductID = 2, Name = "P2"},
                new Product{ProductID = 3, Name = "P3"},
                new Product{ProductID = 4, Name = "P4"},
                new Product{ProductID = 5, Name = "P5"}
            }).AsQueryable());
        var controller = new HomeController(mock.Object) { PageSize = 3 };

        ProductListViewModel model = controller.Index(null, 2).GetViewData<ProductListViewModel>();

        PagingInfo pageInfo = model.PagingInfo;
        Assert.Equal(2, pageInfo.CurrentPage);
        Assert.Equal(3, pageInfo.ItemsPerPage);
        Assert.Equal(5, pageInfo.TotalItems);
        Assert.Equal(2, pageInfo.TotalPages);
    }

    [Fact]
    public void Can_Filter_By_Category()
    {
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns((
             new Product[]
            {
                new Product{ProductID = 1, Name = "P1", Category = "orange"},
                new Product{ProductID = 2, Name = "P2", Category = "orange"},
                new Product{ProductID = 3, Name = "P3", Category = "apple"},
                new Product{ProductID = 4, Name = "P4"}
            }).AsQueryable());

        var controller = new HomeController(mock.Object) { PageSize = 10 };
        var result = controller.Index("orange", productPage: 1).GetViewData<ProductListViewModel>();
        var products = result.Products;

        Assert.True(products.Count == 2);
        Assert.Equal("P1", products[0].Name);
        Assert.Equal("P2", products[1].Name);

    }


}


public static class TestExtensions
{
    public static T GetViewData<T>(this IActionResult actionMethod)
    {
        return (T)(actionMethod as ViewResult).ViewData.Model;
    }
}