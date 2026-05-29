using ChopShop.DataAccess.Data;
using ChopShop.DataAccess.Repository.IRepository;
using ChopShop.Models;

namespace ChopShop.DataAccess.Repository;

public class OrderDetailRepository : Repository<OrderDetail> , IOrderDetailRepository
{
    private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db):base(db)
    {
        _db = db;
    }

    public void Update(OrderDetail orderDetail)
    {
        _db.OrderDetails.Update(orderDetail);
    }
}
