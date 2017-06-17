using System;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Routing;
using UmbracoContentFiles.RequestPipeline;

namespace UmbracoContentFiles.Events
{
    public class Startup : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, FileUrlProvider>();
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Got to catch them all incase Umbraco hasn't been setup... :(
            try
            {
                var contentService = applicationContext.Services.ContentService;
                var textFileContentEvents = new FileContentEvents(contentService);
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
            catch (Exception ex)
            {
                // Umbraco is most likely not installed yet!
                LogHelper.Error<Startup>("Unable to create doc type for text file because Umbraco is most likely not installed!",
                    ex);
            }
        }
    }
}