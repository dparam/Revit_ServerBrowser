using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit_ServerBrowser.App.Helpers;
using Revit_ServerBrowser.App.Plugins.RevitServerBrowser.UserControls.Views;
using Revit_ServerBrowser.App.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Revit_ServerBrowser.App.Plugins.RevitServerBrowser.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command_RSB_ShowWindow : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApplication = commandData.Application;
            Document document = uiApplication.ActiveUIDocument.Document;

            if (document.IsFamilyDocument)
            {
                MessageBox.Show("Плагин работает только с моделями");
                return Result.Failed;
            }

            //

            Window_RSB window = new Window_RSB(uiApplication);
            window.ShowDialog();

            //

            return Result.Succeeded;
        }
    }
}
