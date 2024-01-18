using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit_ServerBrowser.App.Helpers;
using Revit_ServerBrowser.App.Plugins.RevitServerBrowser.UserControls.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;

namespace Revit_ServerBrowser.App.Plugins.RevitServerBrowser.UserControls.ViewModels
{
    // ExampleCollection

    //public class ExampleViewModel
    //{
    //    public SmartCollection<ModelItem> ModelItemsCollection { get; set; }
    //    public ICollectionView CollectionView { get; set; }


    //    public ExampleViewModel()
    //    {
    //        ModelItemsCollection = new SmartCollection<ModelItem>();
    //        CollectionView = CollectionViewSource.GetDefaultView(ModelItemsCollection);

    //        ModelItem root = new ModelItem()
    //        {
    //            Title = "Item 1",
    //            Items = new SmartCollection<ModelItem>()
    //            {
    //                new ModelItem() {Title="Item 1.1"},
    //                new ModelItem() {Title="Item 1.2"},
    //                new ModelItem() {
    //                    Title="Item 1.3",
    //                    Items = new SmartCollection<ModelItem>()
    //                    {
    //                        new ModelItem() {Title="Item 1.3.1"},
    //                        new ModelItem() {Title="Item 1.3.2"},
    //                    }
    //                }
    //            }
    //        };

    //        ModelItemsCollection.Add(root);
    //    }
    //}


    //


    public class MainViewModel : INotifyPropertyChanged
    {
        private UIApplication _uiApplication = null;
        private MemoryStream _memoryStream = new MemoryStream();


        public SmartCollection<ModelItem> ModelItemsCollection { get; set; }
        public ICollectionView CollectionView { get; set; }


        private string _searchString;
        public string SearchPredicate
        {
            get => _searchString;
            set
            {
                _searchString = value;
                FilterCollectionView(value);
                OnPropertyChanged(nameof(SearchPredicate));
            }
        }

        private string _fullPathString;
        public string FullPathString
        {
            get => _fullPathString;
            set
            {
                _fullPathString = value;
                OnPropertyChanged(nameof(FullPathString));
            }
        }


        public bool IsOpenWithAudit { get; set; } = false;

        private bool _isDetachFromCentral;
        public bool IsDetachFromCentral
        {
            get => _isDetachFromCentral;
            set
            {
                _isDetachFromCentral = value;

                if (IsCreateNewLocal == true && value == true)
                    IsCreateNewLocal = false;

                OnPropertyChanged(nameof(IsDetachFromCentral));
            }
        }


        private bool _isCreateNewLocal;
        public bool IsCreateNewLocal
        {
            get => _isCreateNewLocal;
            set
            {
                _isCreateNewLocal = value;

                if (IsDetachFromCentral == true && value == true)
                    IsDetachFromCentral = false;

                OnPropertyChanged(nameof(IsCreateNewLocal));
            }
        }


        // INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        // Constructor

        public MainViewModel(UIApplication uIApplication)
        {
            _uiApplication = uIApplication;
            ModelItemsCollection = new SmartCollection<ModelItem>();
            CollectionView = CollectionViewSource.GetDefaultView(ModelItemsCollection);

            IsDetachFromCentral = true;

            UpdateCollection();
        }


        // SearchFilter

        private void FilterCollectionView(string filterString)
        {
            UpdateCollection(false);

            if (filterString == "" || filterString == " ") return;

            FilterBranch(filterString, ModelItemsCollection[0]);
        }


        private ModelItem FilterBranch(string filterString, ModelItem parentItem, ModelItem archParentItem = null)
        {
            foreach (ModelItem childItem in parentItem.Items.ToList())
            {
                if (childItem.Items.Count == 0)
                {
                    List<string> searchStringList = SearchHelpers.CreateSearchStringList(filterString);
                    List<string> itemStringList = SearchHelpers.CreateItemStringList(childItem.Path);

                    if (!SearchHelpers.CompareStringLists(searchStringList, itemStringList))
                        parentItem.Items.Remove(childItem);

                    continue;
                }

                FilterBranch(filterString, childItem, parentItem);
            }

            if (parentItem.Items.Count == 0)
                archParentItem.Items.Remove(parentItem);

            return null;
        }



        // Update

        private void UpdateCollection(bool reset = true)
        {
            _memoryStream.Position = 0;

            string serverPath = "\\\\REVIT-SERVER\\d\\Projects\\";

            XmlSerializer xs = new XmlSerializer(typeof(SmartCollection<ModelItem>));

            SmartCollection<ModelItem> collection = new SmartCollection<ModelItem>();
            SmartCollection<ModelItem> newCollection = new SmartCollection<ModelItem>();

            if (reset)
            {
                //MessageBox.Show($"GetItems");
                collection.Add(new ModelItem() { Title = "REVIT-SERVER", Path = "REVIT-SERVER", Items = ElementCollector.GetItems(serverPath) });

                xs.Serialize(_memoryStream, collection);
                _memoryStream.Position = 0;

                newCollection = ((SmartCollection<ModelItem>)xs.Deserialize(_memoryStream));
            }
            else
            {
                newCollection = ((SmartCollection<ModelItem>)xs.Deserialize(_memoryStream));
            }

            ModelItem root = new ModelItem() { Title = "RevitServer", Items = newCollection };
            ModelItemsCollection.Reset(new List<ModelItem> { root });
        }


        // Events

        internal void OnSelected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            ModelItem modelItem = item.DataContext as ModelItem;

            if (modelItem == null) return;

            FullPathString = modelItem.Path;
        }


        internal void OnClearSearchString(object sender, RoutedEventArgs e)
        {
            SearchPredicate = "";
        }

        internal void OnOpenModel(object sender, RoutedEventArgs e)
        {
            RevitModelManager.OpenDocumentByPath(_uiApplication, FullPathString, IsOpenWithAudit, IsDetachFromCentral, IsCreateNewLocal);
        }
    }
}
