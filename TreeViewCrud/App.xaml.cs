using System.Configuration;
using System.Data;
using System.Windows;
using TreeViewCrud.Models;
using TreeViewCrud.Services;
using TreeViewCrud.Views;

namespace TreeViewCrud
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        public static AppUser CurrentUser { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //DatabaseInitializer.DropAllTables();
            DatabaseInitializer.EnsureDatabaseCreated();

        }
    }

}
