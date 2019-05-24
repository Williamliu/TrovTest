using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GildedRose.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GildedRose.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private TrovTestDB TestDB { get; set; }
        // dependence inject: TrovTestDB
        public HomeController(TrovTestDB db)
        {
            this.TestDB = db;
        }
        [HttpGet("GetItem")]
        [AllowAnonymous]
        public IActionResult GetItem()
        {
            if (this.TestDB.Items.Count()>0)
                return Ok(this.TestDB.Items);
            else
                return BadRequest("No item found");
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login(PostUser user)
        {
            var customer =  this.TestDB.Customers.FirstOrDefault(p => p.UserName == user.UserName && p.Password == user.Password);
            if(customer!=null)
            {
                customer.AccessToken = CreateAuthToken(user.UserName, "Public");
                this.TestDB.SaveChanges();
                return Ok(customer);
            }
            else
            {
                return BadRequest("Invalid User");
            }
        }
        [HttpPost("Checkout")]
        [Authorize]
        public IActionResult Checkout(PostOrder order)
        {
            var customer = this.TestDB.Customers.FirstOrDefault(p => p.AccessToken == order.AccessToken);
            if(customer!=null)
            {
                Order newOrder = new Order();
                newOrder.Customer = customer;
                newOrder.OrderDateTime = DateTime.Now;
                foreach(var cartItem in order.Cart.Items)
                {
                    var item = this.TestDB.Items.FirstOrDefault(p => p.Id == cartItem.ItemId);
                    if(item!=null)
                    {
                        newOrder.Items.Add(new OrderItem { Item = item,
                            ItemName = item.Name,
                            ItemDescritpion = item.Description,
                            Price = item.Price,
                            Quantity = cartItem.Quantity
                        });
                    }
                }
                this.TestDB.Orders.Add(newOrder);
                this.TestDB.SaveChanges();
                return Ok(this.TestDB.Orders);
            }
            else
            {
                return BadRequest("Invalid AccessToken");
            }
        }

        private string CreateAuthToken(string user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("936DA01F-9ABD-4d9d-80C7-02AF85C822A8");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, user),
                                new Claim(ClaimTypes.Role, role)
                            }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }

}