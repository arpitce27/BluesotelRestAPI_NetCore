using BluesotelRestAPI_NetCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Models
{
    public class Room : Resource
    {
        [Sortable]
        public string Name { get; set; }

        [Sortable(Default = true)]
        public decimal Rate { get; set; }
    }
}
