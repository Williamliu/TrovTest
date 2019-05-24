using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Models
{
    [Table("Order")]
    public class Order
    {
        public Order()
        {
            this.Items = new HashSet<OrderItem>();
            this.Customer = new Customer();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDateTime { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public ICollection<OrderItem> Items { get; set; }
    }
}
