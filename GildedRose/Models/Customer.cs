using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string UserName { get; set; }
        [JsonIgnore]
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string AccessToken { get; set; }
    }
    public class PostUser
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
    }
}
