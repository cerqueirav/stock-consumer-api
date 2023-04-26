using System.ComponentModel.DataAnnotations;

namespace stock_consumer_api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int ClientId { get; set; }
        public DateTime CreationDate { get; set; }
        public int SellerId { get; set; }
        public int StateId { get; set; }
    }
}
