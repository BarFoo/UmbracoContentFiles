using System.IO;
using System.Linq;
using System.Web.Hosting;
using Umbraco.Core.Events;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace UmbracoContentFiles.Events
{
    public class FileContentEvents
    {
        protected const string TextFileContentField = "textFileContent";
        protected string RootApplicationPath { get; set; }
        protected IContentService ContentService { get; set; }

        public FileContentEvents(IContentService contentService)
        {
            ContentService = contentService;
            RootApplicationPath = HostingEnvironment.ApplicationPhysicalPath;
        }

        /// <summary>
        /// Synch the text file content to a physical path when published
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Published(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            LogHelper.Info<FileContentEvents>("Published event for UmbracoTextFiles");
            foreach (var ent in e.PublishedEntities.Where(x => x.HasProperty(TextFileContentField)))
            {
                var path = GetPath(ent);

                System.IO.File.WriteAllText(path, ent.GetValue<string>(TextFileContentField));

                // Force the content node name to end with .txt
                if (!ent.Name.EndsWith(".txt"))
                {
                    ent.Name = ent.Name + ".txt";
                    ContentService.SaveAndPublishWithStatus(ent, 0, false);
                }
                
            }
        }

        /// <summary>
        /// Remove matching text files when unpublished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            LogHelper.Info<FileContentEvents>("UnPublished event for UmbracoTextFiles");
            foreach (var ent in e.PublishedEntities.Where(x => x.HasProperty(TextFileContentField)))
            {
                var path = GetPath(ent);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                else
                {
                    LogHelper.Warn(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Path {0} for text file not found", () => path);
                }
            }
        }

        /// <summary>
        /// Supports file paths at any level
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected string GetPath(IContent content)
        {
            // Make absolutely sure the name is safe
            var name = content.Name;
            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            var invalidPathChars = Path.GetInvalidPathChars();
            
            name = invalidFileNameChars.Aggregate(name, (current, c) => current.Replace(c, '-')).Replace(" ", "-");
            
            // Work out the full directory and prepend each parent
            var fullPath = name;
            var workingParent = content.Parent();
            while (workingParent != null)
            {
                if (workingParent.ParentId <= 0)
                {
                    break;
                }

                var parentDirectoryName = invalidPathChars
                    .Aggregate(workingParent.Name, (current, c) => current.Replace(c, '-')).Replace(" ", "-");

                fullPath = parentDirectoryName  + "\\" + fullPath;

                workingParent = workingParent.Parent();
            }
            
            // todo: Should this logic even be here? Seems a bit ugly, same with the above
            var fileDirectory = Path.GetDirectoryName(RootApplicationPath + fullPath);
            if (fileDirectory != null && !Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory.ToLower());
            }
            
            // Enforce .txt extension incase the entity path doesn't have one
            return RootApplicationPath + string.Format("{0}.txt", fullPath.ToLower().Replace(".txt", string.Empty));
        }
    }
}