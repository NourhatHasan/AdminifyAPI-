

using System.Net.Http.Headers;
using MyAdminifyApp.Domain.Entities;

namespace MyAdminifyApp.Application.Interfaces
{
    public interface IProdutService
    {

        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        void AddProduct(Product prodct);

        void UpdateProduct(Product prodct);

        void DeleteProduct(int id);
    }
}
