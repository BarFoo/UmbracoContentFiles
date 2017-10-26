using System;
using System.Configuration;
using System.Text;
using Umbraco.Core;
using Umbraco.Core.IO;
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
            // Determine if Umbraco has been configured/installed before continuing..
            if (applicationContext.IsConfigured)
            {
                return;
            }
            
            var contentService = applicationContext.Services.ContentService;
            var textFileContentEvents = new FileContentEvents(contentService);

            ContentService.Published += textFileContentEvents.Published;
            ContentService.UnPublished += textFileContentEvents.UnPublished;

            var contentTypeService = applicationContext.Services.ContentTypeService;

            try
            {
                var textFileContentType = contentTypeService.GetContentType("contentFile");
                if (textFileContentType == null)
                {
                    var textboxMultipleDef = new DataTypeDefinition(-1, "Umbraco.TextboxMultiple");
                    var fileService = applicationContext.Services.FileService;

                    var template = new Template("Content File", "ContentFile");
                    var sbTemplate = new StringBuilder();
                    sbTemplate.AppendLine("@inherits Umbraco.Web.Mvc.UmbracoViewPage<Umbraco.Web.Models.RenderModel>");
                    sbTemplate.AppendLine("@{ Layout = null; }");
                    sbTemplate.Append("@Model.Content.GetPropertyValue(\"fileContent\")");
                    template.Content = sbTemplate.ToString();

                    fileService.SaveTemplate(template);

                    var contentType = new ContentType(-1)
                    {
                        Alias = "contentFile",
                        Name = "Content File",
                        Icon = "icon-files",
                        AllowedTemplates = new ITemplate[] { template }
                    };

                    var propertyType = new PropertyType(textboxMultipleDef, "fileContent");
                    propertyType.Name = "Content";

                    contentType.AddPropertyGroup("Content");
                    contentType.AddPropertyType(propertyType, "Content");

                    contentTypeService.Save(contentType);
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error<Startup>("Cannot setup file content type", ex);
            }
        }
    }
}