using ChopShop.DataAccess.Data;
using ChopShop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChopShop.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public ICategoryRepository Category { get; private set; }
    public IProductRepository Product { get; private set; }
    public IShoppingCartRepository ShoppingCart { get; private set; }
    public IOrderHeaderRepository OrderHeader { get; private set; }
    public IOrderDetailRepository OrderDetail { get; private set; }
    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Category = new CategoryRepository(_db);
        Product = new ProductRepository(_db);
        ShoppingCart = new ShoppingCartRepository(_db);
        OrderHeader = new OrderHeaderRepository(_db);
        OrderDetail = new OrderDetailRepository(_db);
    }

    public void Save()
    {
         _db.SaveChanges();
    }
}
