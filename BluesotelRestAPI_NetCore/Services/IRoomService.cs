using BluesotelRestAPI_NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Services
{
    public interface IRoomService
    {
        Task<Room> GetRoomAsync(Guid id);
    }
}
