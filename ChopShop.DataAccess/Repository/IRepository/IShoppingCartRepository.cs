using ChopShop.Models;

namespace ChopShop.DataAccess.Repository.IRepository;

public interface IShoppingCartRepository:IRepository<ShoppingCart>
{
    void Update(ShoppingCart shoppingCart);
}
