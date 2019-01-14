using BluesotelRestAPI_NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Services
{
    public interface IDateLogicService
    {
        DateTimeOffset AlignStartTime(DateTimeOffset date);

        TimeSpan GetMinimumStay();

        DateTimeOffset FurthestPossibleBooking(DateTimeOffset now);

        IEnumerable<BookingRange> GetAllSlots(
            DateTimeOffset start,
            DateTimeOffset? end = null);

        bool DoesConflict(BookingRange b, DateTimeOffset start, DateTimeOffset end);
    }
}
