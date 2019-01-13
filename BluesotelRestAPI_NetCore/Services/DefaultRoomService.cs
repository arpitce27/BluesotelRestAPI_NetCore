using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BluesotelRestAPI_NetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace BluesotelRestAPI_NetCore.Services
{
    public class DefaultRoomService : IRoomService
    {
        private readonly HotelApiDbContext _context;
        private readonly IMapper _mapper;
        public DefaultRoomService(HotelApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Room> GetRoomAsync(Guid id)
        {
            var entity = await _context.Rooms.SingleOrDefaultAsync(r => r.Id == id);
            if (entity == null)
                return null;

            //return new Room
            //{
            //    Href = null, //Url.Link(nameof(GetRoomById), new { roomId = entity.Id }),
            //    Name = entity.Name,
            //    Rate = entity.Rate / 100.0m
            //};

            // this mapping will reduce the rewritting of above mentioned code
            return _mapper.Map<Room>(entity);
        }
    }
}
