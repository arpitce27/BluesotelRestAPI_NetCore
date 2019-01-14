using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Models
{
    public class OpeningEntity : BookingRange
    {
        public Guid RoomId { get; set; }

        public int Rate { get; set; }
    }
}
