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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project2
{
    /// <summary>
    /// Interaction logic for No_of_players_phone.xaml
    /// </summary>
    public partial class No_of_players_phone : Page
    {
        public No_of_players_phone()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int No_of_players = Int32.Parse(((Button)sender).Content.ToString());
            Presenter.NumOfPlayerClick(No_of_players);
        }
    }
}
