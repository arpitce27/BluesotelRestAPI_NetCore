using BluesotelRestAPI_NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Services
{
    public interface IBookingService
    {
        Task<Booking> GetBookingAsync(Guid bookingId);

        Task<Guid> CreateBookingAsync(Guid userId, Guid roomId, DateTimeOffset startAt, DateTimeOffset endAt);
    }
}
