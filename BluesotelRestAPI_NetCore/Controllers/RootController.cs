﻿using BluesotelRestAPI_NetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Controllers
{
    [Route("/")]
    [ApiController]
    public class RootController: ControllerBase
    {
        [HttpGet (Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var response = new RootResponse
            {
                Self = Link.To(nameof(GetRoot)),
                Rooms = Link.ToCollection(nameof(RoomsController.GetAllRooms)),
                Info = Link.To(nameof(InfoController.GetInfo))
            };
            return Ok(response);
        }
    }
}
