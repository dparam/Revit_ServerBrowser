using Autodesk.Revit.UI;
using Revit_ServerBrowser.App.Ribbons;
using System.Collections.Generic;


namespace Revit_ServerBrowser.App
{
    public class MainApp : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication uiControlledApp)
        {
            string tabName = "Revit_ServerBrowser";

            RibbonPanel ribbonPanel = uiControlledApp.CreateRibbonPanel(tabName);
            CreateRibbonPanel(uiControlledApp, ribbonPanel);
            return Result.Succeeded;
        }


        public Result OnShutdown(UIControlledApplication uiControlledApp)
        {
            return Result.Succeeded;
        }


        //


        public void CreateRibbonPanel(UIControlledApplication uiControlledApp, RibbonPanel ribbonPanel)
        {
            var b1 = RibbonPanelHelpers.CreateButton(
                "BTN_ServerBrowser",
                "RSB",
                "Поиск моделей на RevitServer",
                "Revit_ServerBrowser.App.Plugins.RevitServerBrowser.Commands.Command_RSB_ShowWindow",
                "icon_Default.ico");

            ribbonPanel.AddItem(b1);
        }
    }
}
