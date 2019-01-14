using BluesotelRestAPI_NetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly HotelInfo _hotelInfo;
        public InfoController(IOptions<HotelInfo> hoteinfoWrapper)
        {
            _hotelInfo = hoteinfoWrapper.Value;
        }
        [HttpGet(Name = nameof(GetInfo))]
        [ProducesResponseType(200)]
        public ActionResult<HotelInfo> GetInfo()
        {
            _hotelInfo.Href = Url.Link(nameof(GetInfo), null);
            return _hotelInfo;
        }
    }
}
