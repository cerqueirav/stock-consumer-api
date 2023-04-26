using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using stock_consumer_api.Context;
using stock_consumer_api.Models;
using System.Text;

namespace stock_consumer_api.Consumer
{
    public class RabbitConsumer: BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private IConfiguration _configuration;
        private readonly OrderContext _orderContext;

        public RabbitConsumer(IServiceProvider serviceProvider, IConfiguration configuration, OrderContext orderContext)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _orderContext = orderContext;

            var factory = new ConnectionFactory
            {
                Uri = new Uri(_configuration.GetSection("ConnectionRabbit").GetSection("HostName").Value),
                UserName = (_configuration.GetSection("ConnectionRabbit").GetSection("Username").Value),
                Password = (_configuration.GetSection("ConnectionRabbit").GetSection("Password").Value)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: "Order",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonConvert.DeserializeObject<Order>(contentString);

                NotifyUser(message);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume("Order", false, consumer);

            return Task.CompletedTask;
        }

        public void NotifyUser(Order message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                if (message.Amount > 5000)
                {
                    OrderConfirmed orderConfirmed = new OrderConfirmed
                    {
                        OrderId = message.Id,
                        CreationDate = message.CreationDate,
                        ConfirmationDate = DateTime.UtcNow
                    };
                    _orderContext.OrdersConfirmed.Add(orderConfirmed);
                    _orderContext.SaveChanges();

                    //var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                    //notificationService.NotifyUser(orderConfirmed);
                }
            }
        }
    }
}
