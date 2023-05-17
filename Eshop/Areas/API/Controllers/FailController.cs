using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FailController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Bạn chưa đăng nhập!";
        }
    }
}
