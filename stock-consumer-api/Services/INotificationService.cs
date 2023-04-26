using stock_consumer_api.Models;

namespace stock_consumer_api.Services
{
    public interface INotificationService
    {
        void NotifyUser(OrderConfirmed orderConfirmed);
    }
}
