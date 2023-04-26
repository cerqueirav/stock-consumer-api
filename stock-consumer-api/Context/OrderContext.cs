using Microsoft.EntityFrameworkCore;
using stock_consumer_api.Models;
using System.Collections.Generic;

namespace stock_consumer_api.Context
{
    public class OrderContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public OrderContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderConfirmed> OrdersConfirmed { get; set; }
    }
}
