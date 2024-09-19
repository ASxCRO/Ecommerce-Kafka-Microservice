using Ecommerce.Model;
using Ecommerce.OrderService.Data;
using Ecommerce.OrderService.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(OrderDbContext orderDbContext, IKafkaProducer kafkaProducer) : ControllerBase
    {
        [HttpGet]
        public async Task<List<OrderModel>> GetOrders()
        {
            return await orderDbContext.Orders.ToListAsync();
        }

        [HttpPost]
        public async Task<OrderModel> Create(OrderModel model)
        {
            model.OrderDate = DateTime.Now;
            await orderDbContext.Orders.AddAsync(model);
            await orderDbContext.SaveChangesAsync();
            
            await kafkaProducer.ProduceAsync("order-topic", new Confluent.Kafka.Message<string, string>
            {
                Key=model.Id.ToString(),
                Value=System.Text.Json.JsonSerializer.Serialize(model)
            });

            return model;
        }
    }
}
