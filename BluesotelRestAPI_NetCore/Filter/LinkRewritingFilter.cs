using BluesotelRestAPI_NetCore.Helper;
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
            bool dontfilterLinksif = asObjectResults?.StatusCode >= 400
                || asObjectResults?.Value == null
                || asObjectResults?.Value as Resource == null;

            if (dontfilterLinksif)
            {
                return next();
            }

            // Otherwise rewrite the links
            var rewritter = new LinkRewritter(_urlHelperFactory.GetUrlHelper(context));
            RewriteAllLinks(asObjectResults.Value, rewritter);

            return next();
        }

        private static void RewriteAllLinks(object model, LinkRewritter rewriter)
        {
            if (model == null) return;

            var allProperties = model
                .GetType().GetTypeInfo()
                .GetAllProperties()
                .Where(p => p.CanRead)
                .ToArray();

            var linkProperties = allProperties.Where(p => p.CanWrite & p.PropertyType == typeof(Link));

            // For rewriting the links properties
            foreach (var linkProperty in linkProperties)
            {
                var rewritten = rewriter.Rewrite(linkProperty.GetValue(model) as Link);
                if (rewritten == null) continue;

                linkProperty.SetValue(model, rewritten);

                // Handeling of the hidden self property to ROOT object
                if (linkProperty.Name == nameof(Resource.Self))
                {
                    allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Href))
                            ?.SetValue(model, rewritten.Href);

                    allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Method))
                            ?.SetValue(model, rewritten.Method);

                    allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Relations))
                            ?.SetValue(model, rewritten.Relations);
                }
            }

            // For rewriting the links in arrays
            var arrayProperties = allProperties.Where(p => p.PropertyType.IsArray);
            ReWriteLinksInArray(arrayProperties, model, rewriter);

            // For rewriting the links in objects
            var objectProperties = allProperties.Except(linkProperties).Except(arrayProperties);
            ReWriteLinksInNeastedObjects(objectProperties, model, rewriter);

        }

        // Rewritting the links which are in neasted objects sucb as class and all
        private static void ReWriteLinksInNeastedObjects(IEnumerable<PropertyInfo> objectProperties, object model, LinkRewritter rewriter)
        {
            foreach (var objectProperty in objectProperties)
            {
                if (objectProperty.PropertyType == typeof(string))
                    continue;

                var typeInfo = objectProperty.PropertyType.GetTypeInfo();
                if (typeInfo.IsClass)
                    RewriteAllLinks(objectProperty.GetValue(model), rewriter);
            }
        }

        // Rewritting the links whicha are in array elememts
        private static void ReWriteLinksInArray(IEnumerable<PropertyInfo> arrayProperties, object model, LinkRewritter rewriter)
        {
            foreach (var arrayProperty in arrayProperties)
            {
                var array = arrayProperty.GetValue(model) as Array ?? new Array[0];
                foreach (var item in array)
                {
                    RewriteAllLinks(item, rewriter);
                }
            }
        }
    }
}
