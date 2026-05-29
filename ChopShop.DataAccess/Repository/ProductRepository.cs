using ChopShop.DataAccess.Data;
using ChopShop.DataAccess.Repository.IRepository;
using ChopShop.Models;

namespace ChopShop.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _db;
    public ProductRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }



    public void Update(Product product)
    {
        var productFromDb = _db.Products.FirstOrDefault(p => p.Id == product.Id);
        if (productFromDb != null)
        {
            productFromDb.Title = product.Title;
            productFromDb.Description = product.Description;
            productFromDb.Price = product.Price;
            productFromDb.CategoryId = product.CategoryId;
        }
        if (product.ImageUrl != null)
        {
            productFromDb.ImageUrl = product.ImageUrl;
        }
    }
    public List<Product> Search(string searchTerm)
    {
        searchTerm = searchTerm.ToLower();
        var product = _db.Products
                .Where(p =>
                    p.Title.ToLower().Contains(searchTerm) ||
                    p.Description.ToLower().Contains(searchTerm) ||
                    p.Category.Name.ToLower().Contains(searchTerm))
                .ToList();
        return product;
    }
}
