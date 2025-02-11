

using MyAdminifyApp.Application.Interfaces;
using MyAdminifyApp.Domain.Entities;

namespace MyAdminifyApp.Application.Services
{
    public class ProductService : IProdutService
    {
        private readonly ProductDbContext _context;

        public ProductService(ProductDbContext context)
        {
            _context = context;
        }

     

        public IEnumerable<Product> GetAllProducts() => _context.Products.ToList();

        public Product GetProductById(int id) => _context.Products.FirstOrDefault(p => p.Id == id);

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
