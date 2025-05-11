using OrderAPI.Models;

namespace OrderAPI.DTOs.Mappings;

public static class OrderDTOMappingExtensions
{
    public static OrderDTO? ToDTO(this Order? orderModel)
    {
        if (orderModel is null) return null;

        return new OrderDTO
        {
            OrderId = orderModel.OrderId,
            CustomerName = orderModel.CustomerName,
            OrderDate = orderModel.OrderDate,
            TotalAmount = orderModel.TotalAmount
        };
    }

    public static Order? ToModel(this OrderDTO? orderDto)
    {
        if (orderDto is null) return null;

        return new Order
        {
            // OrderId = orderDto.OrderId,
            CustomerName = orderDto.CustomerName,
            OrderDate = orderDto.OrderDate,
            TotalAmount = orderDto.TotalAmount
        };
    }

    public static IEnumerable<OrderDTO> ToDtos(this IEnumerable<Order>? orderModels)
    {
        if (orderModels is null || !orderModels.Any())
            return new List<OrderDTO>();
        
        return orderModels.Select(orderModel => new OrderDTO
        {
            OrderId = orderModel.OrderId,
            CustomerName = orderModel.CustomerName,
            OrderDate = orderModel.OrderDate,
            TotalAmount = orderModel.TotalAmount
        }).ToList();
    }
}