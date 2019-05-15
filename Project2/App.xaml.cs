using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Project2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow PC = new MainWindow();
        public static MainWindow_Phone Phone = new MainWindow_Phone();
        public static Console console = new Console();
        public Game newGame = new Game();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            PC.Show();
            console.Show();
            Phone.Show();
            Presenter.GetWindow(PC, Phone, console, newGame);

        }
    }

}
