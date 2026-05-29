using ChopShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChopShop.DataAccess.Repository.IRepository;

public interface IProductRepository : IRepository<Product>
{
    void Update(Product product);
    List<Product> Search(string searchTerm);
}
