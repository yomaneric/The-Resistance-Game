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
    /// Interaction logic for InputPlayerName.xaml
    /// </summary>
    public partial class InputPlayerName : Window
    {
        int pos;
        Game game;
        public InputPlayerName(int pos, Game game)
        {
            InitializeComponent();
            this.game = game;
            this.pos = pos;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            game.InputPlayer(pos, FillName.Text, ErrorMessage, IdentityShow, Enter, identityImage);
        }
    }
}
