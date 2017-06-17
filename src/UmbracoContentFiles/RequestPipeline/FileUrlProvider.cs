using System;
using System.Collections.Generic;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace UmbracoContentFiles.RequestPipeline
{
    /// <summary>
    /// This provider is useful for showing links in the backoffice when the content name contains an extension
    /// </summary>
    public class FileUrlProvider : IUrlProvider
    {
        public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
        {
            // todo: If I could use this method I would, but it always results in a collision error since 7.5+
            return null;
        }

        public IEnumerable<string> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
            var list = new List<string>();

            var content = umbracoContext.ContentCache.GetById(id);
            if (content != null && content.DocumentTypeAlias == "contentFile" && content.Parent != null && content.Name.Contains("."))
            {
                // This may look odd, but this line of code keeps the '.extension' part of the name when displaying this contents URL
                // within link to document or such
                list.Add(content.Parent.Url.TrimEnd('/') + "/" + content.Name.Replace(" ", "-").ToLower());
            }

            return list;
        }
    }
}