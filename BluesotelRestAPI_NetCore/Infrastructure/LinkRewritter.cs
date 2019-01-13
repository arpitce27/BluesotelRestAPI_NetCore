using BluesotelRestAPI_NetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Infrastructure
{
    public class LinkRewritter
    {
        private readonly IUrlHelper _urlHelper;
        public LinkRewritter(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        // Here original parameter is passing the all info 
        // excep the HREF's absolute URL - so using this method we rewrite the URL
        public Link Rewrite(Link original)
        {
            if (original == null) return null;

            return new Link()
            {
                Href = _urlHelper.Link(original.RouteName, original.RouteValue),
                Method = original.Method,
                Relations = original.Relations
            };
        }
    }
}
