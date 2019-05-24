using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Models
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescritpion { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("OrderId")]
        [JsonIgnore] //important: prevent JSON recursive dead loop. Order->OrderItem.Order->OrderItem...
        public Order Order { get; set; }
        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }

    public class PostOrder
    {
        public string AccessToken { get; set; }
        public PostCart Cart { get; set; }
    }

    public class PostCart
    {
        public PostCart()
        {
            this.Items = new List<PostItem>();
        }
        public List<PostItem> Items { get; set; }
    }
    public class PostItem
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
