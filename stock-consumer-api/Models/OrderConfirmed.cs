using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stock_consumer_api.Models
{
    [Table("OrderConfirmed")]
    public class OrderConfirmed
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ConfirmationDate { get; set; } 
    }
}
