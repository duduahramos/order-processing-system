using System.Linq.Expressions;
using OrderAPI.Models;

namespace OrderAPI.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetAsync(Expression<Func<Order, bool>> predicate);
    Order Create(Order orderEntity);
    Order Update(Order orderEntity);
    Order Delete(Order orderEntity);
    Task CommitAsync();
    public void Dispose();
}