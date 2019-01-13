using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Models
{
    public abstract class Resource
    {
        [JsonProperty(Order = -2)] //which sets the HREF at the top when reading the APIs
        public string Href { get; set; }
    }
}
