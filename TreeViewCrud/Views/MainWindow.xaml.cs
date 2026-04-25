using System.Windows;
using TreeViewCrud.Views;

namespace TreeViewCrud;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var loginWindow = new LoginRegisterWindow();
        if (loginWindow.ShowDialog() != true)
        {
            // Пользователь закрыл окно или нажал отмену

            return;
        }

        // Сохраняем текущего пользователя, например, в статическом свойстве
        App.CurrentUser = loginWindow.CurrentUser;
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
        // Сброс текущего пользователя
        App.CurrentUser = null;
        // Показываем окно входа снова
        var loginWindow = new LoginRegisterWindow();
        if (loginWindow.ShowDialog() == true)
        {
            App.CurrentUser = loginWindow.CurrentUser;
            // Обновляем отображение
            UserInfo.Text = $"{App.CurrentUser.LastName} {App.CurrentUser.FirstName} ({App.CurrentUser.Role})";
        }
        else
        {
            Application.Current.Shutdown();
        }
    }
}
