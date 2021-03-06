﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Models
{
    public abstract class Resource: Link
    {
        [JsonIgnore]
        public Link Self { get; set; }
    }
}
