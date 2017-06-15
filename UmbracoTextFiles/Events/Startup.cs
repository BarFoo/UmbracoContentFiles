using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Routing;
using UmbracoTextFiles.RequestPipeline;

namespace UmbracoTextFiles.Events
{
    public class Startup : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, TextFileUrlProvider>();
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var contentService = applicationContext.Services.ContentService;
            var textFileContentEvents = new TextFileContentEvents(contentService);
            ContentService.Published += textFileContentEvents.Published;
            ContentService.UnPublished += textFileContentEvents.UnPublished;
            
            var contentTypeService = applicationContext.Services.ContentTypeService;
            var textFileContentType = contentTypeService.GetContentType("textFile");
            if (textFileContentType == null)
            {
                var textboxMultipleDef = new DataTypeDefinition(-1, "Umbraco.TextboxMultiple");

                var contentType = new ContentType(-1)
                {
                    Alias = "textFile",
                    Name = "Text File",
                    Icon = "icon-files"
                };

                var propertyType = new PropertyType(textboxMultipleDef, "textFileContent");
                propertyType.Name = "Content";

                contentType.AddPropertyGroup("File Content");
                contentType.AddPropertyType(propertyType, "File Content");

                contentTypeService.Save(contentType);
            }
        }
    }
}