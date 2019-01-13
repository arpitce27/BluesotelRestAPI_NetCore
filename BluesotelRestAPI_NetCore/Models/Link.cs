using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Models
{
    public class Link
    {
        public const string GetMethod = "GET";

        // Returns the defaule Link object to service class
        public static Link To(string routeName, object routeValues = null)
            => new Link()
            {
                RouteName = routeName,
                RouteValue = routeValues,
                Method = GetMethod,
                Relations = null
            };

        [JsonProperty(Order = -4)]
        public string Href { get; set; }

        [JsonProperty(Order = -3, 
            PropertyName = "rel", 
            NullValueHandling = NullValueHandling.Ignore)]
        public string[] Relations { get; set; }

        [JsonProperty(Order = -2, 
            DefaultValueHandling = DefaultValueHandling.Ignore, 
            NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(GetMethod)]
        public string Method { get; set; }

        // it will store the route name before rewritten by LinkRewritterFilter
        [JsonIgnore]
        public string RouteName { get; set; }

        // it will store the route parameters before rewritten by LinkRewritterFilter
        [JsonIgnore]
        public object RouteValue { get; set; }
    }
}
