using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using TreeViewCrud.Models;
using TreeViewCrud.Services;
using TreeViewCrud.ViewModels;
using TreeViewCrud.Views;

namespace TreeViewCrud;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var loginWindow = new LoginRegisterWindow();
        if (loginWindow.ShowDialog() != true)
        {
            App.Current.Shutdown();
            return;
        }
        App.CurrentUser = AuthenticationService.CurrentUser;
        if (App.CurrentUser != null)
        {
            UserInfo.Text = $"{App.CurrentUser.LastName} {App.CurrentUser.FirstName} ({App.CurrentUser.Role})";

            if (App.CurrentUser.Role == "admin")
                UsersButton.Visibility = Visibility.Visible;
        }
    }

    private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (DataContext is ViewModels.MainViewModel viewModel)
        {
            viewModel.SelectedItem = e.NewValue;
        }
    }
    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        App.CurrentUser = null;
        var loginWindow = new LoginRegisterWindow();
        if (loginWindow.ShowDialog() == true)
        {
            App.CurrentUser = AuthenticationService.CurrentUser;
            UserInfo.Text = $"{App.CurrentUser.LastName} {App.CurrentUser.FirstName} ({App.CurrentUser.Role})";
            UsersButton.Visibility = App.CurrentUser.Role == "admin" ? Visibility.Visible : Visibility.Collapsed;
        }
        else
        {
            Application.Current.Shutdown();
        }
    }

    private void UsersButton_Click(object sender, RoutedEventArgs e)
    {

        var usersWindow = new UsersManagementWindow();
        usersWindow.Owner = this;
        usersWindow.ShowDialog();
    }
    private void TreeView_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed) return;
        var source = FindAncestor<TreeViewItem>(e.OriginalSource as DependencyObject);
        if (source?.DataContext is Item draggedItem)
            DragDrop.DoDragDrop(source, draggedItem, DragDropEffects.Move);
        else if (source?.DataContext is Batch draggedBatch)
            DragDrop.DoDragDrop(source, draggedBatch, DragDropEffects.Move);
    }

    private void TreeView_DragEnter(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.Move;
    }

    private void TreeView_DragOver(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }




    private void TreeView_Drop(object sender, DragEventArgs e)
    {
        var targetElement = FindAncestor<TreeViewItem>(e.OriginalSource as DependencyObject);
        var target = targetElement?.DataContext;
        var draggedItem = e.Data.GetData(typeof(Item)) as Item;
        var draggedBatch = e.Data.GetData(typeof(Batch)) as Batch;

        var vm = DataContext as MainViewModel;
        if (vm == null) return;

        if (draggedItem != null && target is Category targetCategory)
            vm.MoveItemToCategory(draggedItem, targetCategory);
        else if (draggedBatch != null && target is Item targetItem)
            vm.MoveBatchToItem(draggedBatch, targetItem);
    }
    private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        while (current != null)
        {
            if (current is T t) return t;
            if (current is Visual || current is Visual3D)
                current = VisualTreeHelper.GetParent(current);
            else
                current = LogicalTreeHelper.GetParent(current);
        }
        return null;
    }

}
