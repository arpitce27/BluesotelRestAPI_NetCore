using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Models
{
    public class RootResponse: Resource
    {
        public Link Info { get; set; }

        public Link Rooms { get; set; }
    }
}
