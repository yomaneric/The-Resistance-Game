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
    /// Interaction logic for SuccessFail.xaml
    /// </summary>
    public partial class SuccessFail : Window
    {
        Game game;
        public SuccessFail(int i, Game game)
        {
            InitializeComponent();
            this.game = game;
            display.Text = string.Format("{0}, please choose Success or Fail", game.playerlist[i].name);
        }


        private void Fail_Click(object sender, RoutedEventArgs e)
        {
            game.FailCount++;
            Close();
        }

        private void Success_Click(object sender, RoutedEventArgs e)
        {
            game.SuccessCount++;
            Close();
        }
    }
}
