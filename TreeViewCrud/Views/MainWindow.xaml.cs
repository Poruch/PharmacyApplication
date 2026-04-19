using System.Windows;

namespace TreeViewCrud;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (DataContext is ViewModels.MainViewModel viewModel)
        {
            viewModel.SelectedItem = e.NewValue;
        }
    }
}
