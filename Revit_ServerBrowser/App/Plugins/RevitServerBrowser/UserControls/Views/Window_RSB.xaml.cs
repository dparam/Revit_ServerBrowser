using Autodesk.Revit.UI;
using Revit_ServerBrowser.App.Plugins.RevitServerBrowser.UserControls.ViewModels;
using System.Windows;
using System.Windows.Input;
using TextBox = System.Windows.Controls.TextBox;


namespace Revit_ServerBrowser.App.Plugins.RevitServerBrowser.UserControls.Views
{
    public partial class Window_RSB : Window
    {
        private MainViewModel _mainViewModel = null;


        public Window_RSB(UIApplication uiApplication)
        {
            InitializeComponent();
            _mainViewModel = new MainViewModel(uiApplication);

            DataContext = _mainViewModel;
        }


        // Events

        private void OnSelected(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            _mainViewModel.OnSelected(sender, e);
        }

        private void TextBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (e.ClickCount == 3)
                textBox.SelectAll();
        }

        private void OnSelectStringPath(object sender, RoutedEventArgs e)
        {
            this.pathTextBox.Focus();
            this.pathTextBox.SelectAll();
        }

        private void OnClearSearchString(object sender, RoutedEventArgs e)
        {
            _mainViewModel.OnClearSearchString(sender, e);
        }

        private void OnOpenModel(object sender, RoutedEventArgs e)
        {
            this.Close();
            _mainViewModel.OnOpenModel(sender, e);
        }
    }
}
