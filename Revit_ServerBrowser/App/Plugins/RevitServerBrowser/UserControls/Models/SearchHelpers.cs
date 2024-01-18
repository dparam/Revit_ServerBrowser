using Autodesk.Revit.DB;
using Revit_ServerBrowser.App.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Revit_ServerBrowser.App.Plugins.RevitServerBrowser.UserControls.Models
{
    public static class SearchHelpers
    {
        public static List<string> CreateSearchStringList(string searchString)
        {
            char[] splits = new char[] { ' ', '_', ',', '.', ':', '"', '*', '+', '(', ')', '[', ']' };
            List<string> searchList = searchString.ToLower().Split(splits).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            return searchList;
        }


        public static List<string> CreateItemStringList(string searchString)
        {
            char[] splits = new char[] { ' ', '_', ',', '.', ':', '"', '*', '+', '(', ')', '[', ']' };
            List<string> searchList = searchString.ToLower().Split(splits).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            return searchList;
        }


        public static bool CompareStringLists(List<string> searchStringList, List<string> elementStringList)
        {
            foreach (string search in searchStringList)
            {
                if (elementStringList.Any(element => element.Contains(search)) == false)
                    return false;
            }

            return true;
        }


        public static string GetItemIcon(bool isRVT)
        {
            string ItemIcon = $"{AppData.RESOURCES}icon_Folder.ico";

            if (isRVT == true) ItemIcon = $"{AppData.RESOURCES}icon_RVT.ico";
            if (isRVT == false) ItemIcon = $"{AppData.RESOURCES}icon_Folder.ico";

            return ItemIcon;
        }
    }
}
