using Revit_ServerBrowser.App.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;


namespace Revit_ServerBrowser.App.Plugins.RevitServerBrowser.UserControls.Models
{
    public static class ElementCollector
    {
        public static SmartCollection<ModelItem> GetItems(string path)
        {
            var items = new SmartCollection<ModelItem>();
            var dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists) return items;

            try
            {
                foreach (var directory in dirInfo.GetDirectories())
                {
                    if (directory.Name.Contains(".rvt"))
                    {
                        items.Add(new ModelItem
                        {
                            Title = directory.Name,
                            Path = CreatePath(directory.FullName),
                            ItemIcon = SearchHelpers.GetItemIcon(true)
                        });
                    }
                    else
                    {
                        items.Add(new ModelItem
                        {
                            Title = directory.Name,
                            Path = CreatePath(directory.FullName),
                            ItemIcon = SearchHelpers.GetItemIcon(false),
                            Items = GetItems(directory.FullName)
                        });
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                return items;
            }

            return items;
        }


        private static string CreatePath(string path)
        {
            string newPath;

            newPath = path.Replace(@"\", @"/");
            newPath = "RSN:" + newPath;
            newPath = newPath.Replace("d/Projects/", "");

            return newPath;
        }
    }
}
