using Revit_ServerBrowser.App.Helpers;
using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Linq;


namespace Revit_ServerBrowser.App.Plugins.RevitServerBrowser.UserControls.Models
{
    public class ModelItem
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public string ItemIcon { get; set; }

        public SmartCollection<ModelItem> Items { get; set; }
    }
}
