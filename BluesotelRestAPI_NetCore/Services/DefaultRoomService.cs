using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BluesotelRestAPI_NetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace BluesotelRestAPI_NetCore.Services
{
    public class DefaultRoomService : IRoomService
    {
        private readonly HotelApiDbContext _context;
        private readonly IConfigurationProvider _mappingConfiguration;
        public DefaultRoomService(HotelApiDbContext context, IConfigurationProvider mappingConfiguration)
        {
            _context = context;
            _mappingConfiguration = mappingConfiguration;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsnyc()
        {
            var query = _context.Rooms.ProjectTo<Room>(_mappingConfiguration);

            return await query.ToArrayAsync();
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
            var mapper = _mappingConfiguration.CreateMapper();
            return mapper.Map<Room>(entity);
        }

        public async Task<PagedResults<Room>> GetRoomsAsnyc(
            PagingOptions pagingOptions,
            SortOptions<Room, RoomEntity> sortOptions)
        {
            IQueryable<RoomEntity> query = _context.Rooms;
            query = sortOptions.Apply(query);

            var size = await query.CountAsync();

            var pagedRoom = await query.Skip(pagingOptions.Offset.Value)
                                .Take(pagingOptions.Limit.Value)
                                .ProjectTo<Room>(_mappingConfiguration)
                                .ToArrayAsync();

            return new PagedResults<Room>()
            {
                Items = pagedRoom,
                TotalSize = size
            };
        }
    }
}
