using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderAPI.AWSServices.Menssaging;
using OrderAPI.DTOs;
using OrderAPI.DTOs.Mappings;
using OrderAPI.Repositories.Interfaces;

namespace OrderAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly SQSHandler _sqsHandler;

    public OrderController(ILogger<OrderController> logger, IOrderRepository orderRepository,  SQSHandler sqsHandler)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _sqsHandler = sqsHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> Get()
    {
        var orders = await _orderRepository.GetAllAsync();
        
        if (orders == null || !orders.Any())
        {
            _logger.LogWarning("No orders found.");
            return NotFound("No orders found.");
        }
        
        var orderDtos = orders.ToDtos();

        return Ok(orderDtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDTO>> Get(Guid id)
    {
        var order = await _orderRepository.GetAsync(o => o.OrderId == id);

        if (order == null)
        {
            _logger.LogWarning("Order not found.");
            return NotFound("Order not found.");
        }
        
        var orderDto = order.ToDTO();
        
        return Ok(orderDto);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDTO>> Post(OrderDTO orderDto)
    {
        var order = orderDto.ToModel();

        _orderRepository.Create(order);

        await _orderRepository.CommitAsync();
        
        var createdOrderDto = order.ToDTO();

        await _sqsHandler.SendAsync(JsonConvert.SerializeObject(createdOrderDto));

        return CreatedAtAction(nameof(Get), new { id = createdOrderDto.OrderId }, createdOrderDto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var order = await _orderRepository.GetAsync(o => o.OrderId == id);
        
        if (order == null)
        {
            _logger.LogWarning("Order not found.");
            return NotFound("Order not found.");
        }
        
        _orderRepository.Delete(order);
        await _orderRepository.CommitAsync();
        
        return NoContent();
    }
}