using BluesotelRestAPI_NetCore.Infrastructure;
using BluesotelRestAPI_NetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Filter
{
    public class LinkRewritingFilter : IAsyncResultFilter
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        public LinkRewritingFilter(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }
        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // Getting the model/data returned from controller class
            var asObjectResults = context.Result as ObjectResult;

            // Will not rewrite links if 
            bool dontfilterLinksif = asObjectResults?.StatusCode > 400
                || asObjectResults?.Value == null
                || asObjectResults?.Value as Resource == null;

            if (dontfilterLinksif)
            {
                return next();
            }

            // Otherwise rewrite the links
            var rewritter = new LinkRewritter(_urlHelperFactory.GetUrlHelper(context));
            RewriteAllLinks(asObjectResults, rewritter);

            return next();
        }

        private static void RewriteAllLinks(object model, LinkRewritter rewriter)
        {
            if (model == null) return;

            var allProperties = model
                .GetType().GetTypeInfo()
                .GetProperties()
                .Where(p => p.CanRead)
                .ToArray();

            var linkProperties = allProperties.Where(p => p.CanWrite & p.PropertyType == typeof(Link));

            foreach (var linkProperty in linkProperties)
            {
                var rewritten = rewriter.Rewrite(linkProperty.GetValue(model) as Link);
                if (rewritten == null) continue;


            }

        }
    }
}
