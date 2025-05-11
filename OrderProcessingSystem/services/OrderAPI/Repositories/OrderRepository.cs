using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Models;
using OrderAPI.Repositories.Interfaces;

namespace OrderAPI.Repositories;

public class OrderRepository : IOrderRepository
{
    protected readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Order.AsNoTracking().ToListAsync();
    }

    public Task<Order?> GetAsync(Expression<Func<Order, bool>> predicate)
    {
        return _context.Order.FirstOrDefaultAsync(predicate);
    }

    public Order Create(Order orderEntity)
    {
        _context.Order.Add(orderEntity);

        return orderEntity;
    }

    public Order Update(Order orderEntity)
    {
        _context.Order.Update(orderEntity);
        
        return orderEntity;
    }

    public Order Delete(Order orderEntity)
    {
        _context.Order.Remove(orderEntity);
        
        return orderEntity;
    }
    
    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}