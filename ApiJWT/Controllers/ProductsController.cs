using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiJWT.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiJWT.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //De alta JWT
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<Products> Get()
        {
            var products = new List<Products>()
            {
                new Products{ ProductId = 1, Name = "Producto 1", Price = 100 },
                new Products{ ProductId = 2, Name = "Producto 2", Price = 200 },
                new Products{ ProductId = 3, Name = "Producto 3", Price = 300 },
                new Products{ ProductId = 4, Name = "Producto 4", Price = 400 }
            };

            return products;
        }
    }
}
