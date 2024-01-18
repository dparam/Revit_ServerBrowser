using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit_ServerBrowser.App.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Revit_ServerBrowser.App.Plugins.RevitServerBrowser.UserControls.Models
{
    public static class RevitModelManager
    {
        public static string GetUserLocalCopiesPath(UIApplication uIApplication)
        {
            string revitINIPath = $"{uIApplication.Application.CurrentUsersDataFolderPath}\\Revit.ini";
            INIManager iniManager = new INIManager(revitINIPath);

            string revitLocalPath = iniManager.GetPrivateString("Directories", "ProjectPath");

            return revitLocalPath;
        }


        public static void OpenDocumentByPath(UIApplication uiApplication, string centralPath, bool isAudit, bool isDetached, bool isNewLocal)
        {
            ModelPath modelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(centralPath);
            OpenOptions openOptions = new OpenOptions();

            openOptions.Audit = isAudit;
            openOptions.DetachFromCentralOption = DetachFromCentralOption.DoNotDetach;

            if (isDetached)
            {
                openOptions.DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets;
                OpenDocumentWithOptions(uiApplication, modelPath, openOptions, false);
                return;
            }

            if (isNewLocal)
            {
                //string localPath = GetUserLocalCopiesPath(uiApplication);
                //var dirInfo = new DirectoryInfo(localPath);

                //if (localPath == "" || !dirInfo.Exists)
                //{
                //    MessageBox.Show($"Не удалось определить путь до локальных копий\nlocalPath = {localPath}");
                //    return;
                //}

                //ModelPath localModelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(localPath);
                //WorksharingUtils.CreateNewLocal(modelPath, localModelPath);

                //OpenDocumentWithOptions(uiApplication, localModelPath, openOptions, false);

                MessageBox.Show($"Создание локалных копий ещё в разработке :)");
                return;
            }

            OpenDocumentWithOptions(uiApplication, modelPath, openOptions, false);
        }


        //


        private static void OpenDocumentWithOptions(UIApplication uiApplication, ModelPath modelPath, OpenOptions openOptions, bool detachAndPromt)
        {
            try
            {
                uiApplication.OpenAndActivateDocument(modelPath, openOptions, detachAndPromt);
            }
            catch (Exception exceptionx)
            {
                MessageBox.Show($"{exceptionx}");
                return;
            }
        }
    }
}
