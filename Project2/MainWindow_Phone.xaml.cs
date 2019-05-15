using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project2
{
    /// <summary>
    /// Interaction logic for MainWindow_Phone.xaml
    /// </summary>
    public partial class MainWindow_Phone : Window
    {
        public MainWindow_Phone()
        {
            InitializeComponent();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Presenter.PlayButtonClick();
        }
    }
}
