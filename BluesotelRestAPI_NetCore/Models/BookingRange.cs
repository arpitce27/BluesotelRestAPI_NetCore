using System;

namespace BluesotelRestAPI_NetCore.Models
{
    public class BookingRange
    {
        public DateTimeOffset StartAt { get; set; }

        public DateTimeOffset EndAt { get; set; }
    }
}