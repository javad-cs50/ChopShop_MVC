using ChopShop.DataAccess.Data;
using ChopShop.DataAccess.Repository.IRepository;
using ChopShop.Models;

namespace ChopShop.DataAccess.Repository;

public class CategoryRepository : Repository<Category> , ICategoryRepository
{
    private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db):base(db)
    {
        _db = db;
    }

    public void Update(Category category)
    {
        _db.Categories.Update(category);
    }
}
