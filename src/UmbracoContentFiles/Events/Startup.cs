using System;
using System.Configuration;
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
            // This part needs a try catch incase Umbraco hasn't been setup yet
            // Determine if Umbraco is installed ourselves before continuing to use the content type service
            // This is important because of the ArgumentNullException that will occur in < 7.6 if Umbraco
            // hasn't been setup/installed fully.
            // todo: There must be a better way to handle all of the below..
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["umbracoConfigurationStatus"]))
            {
                return;
            }

            try
            {
                var contentService = applicationContext.Services.ContentService;
                var textFileContentEvents = new FileContentEvents(contentService);
                ContentService.Published += textFileContentEvents.Published;
                ContentService.UnPublished += textFileContentEvents.UnPublished;

                var contentTypeService = applicationContext.Services.ContentTypeService;
                var textFileContentType = contentTypeService.GetContentType("contentFile");
                if (textFileContentType == null)
                {
                    var textboxMultipleDef = new DataTypeDefinition(-1, "Umbraco.TextboxMultiple");

                    var contentType = new ContentType(-1)
                    {
                        Alias = "contentFile",
                        Name = "Content File",
                        Icon = "icon-files"
                    };

                    var propertyType = new PropertyType(textboxMultipleDef, "fileContent");
                    propertyType.Name = "Content";

                    contentType.AddPropertyGroup("Content");
                    contentType.AddPropertyType(propertyType, "Content");

                    contentTypeService.Save(contentType);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<Startup>("Unable to setup content file document type", ex);
            }
        }
    }
}