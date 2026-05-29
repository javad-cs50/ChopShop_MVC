using ChopShop.DataAccess.Data;
using ChopShop.DataAccess.Repository.IRepository;
using ChopShop.Models;

namespace ChopShop.DataAccess.Repository;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
    private readonly ApplicationDbContext _db;
    public OrderHeaderRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(OrderHeader orderHeader)
    {
        _db.OrderHeaders.Update(orderHeader);
    }
}
