using BluesotelRestAPI_NetCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Models
{
    public class Opening
    {
        [Sortable(EntityProperty = nameof(OpeningEntity.RoomId))]
        public Link Room { get; set; }

        [Sortable(Default = true)]
        public DateTimeOffset StartAt { get; set; }

        [Sortable]
        public DateTimeOffset EndAt { get; set; }

        [Sortable]
        public decimal Rate { get; set; }
    }
}
