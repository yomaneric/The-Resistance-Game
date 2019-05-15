using System.Windows;
using System.Windows.Media;

namespace Project2
{
    /// <summary>
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : Window
    {
        public Console()
        {
            InitializeComponent();
            box.Visibility = Visibility.Hidden;
        }
        public int stage = -2;
        private void Stage1(string input)
        {
            int No_of_players;
            int.TryParse(input, out No_of_players);
            if (No_of_players >= 5 && No_of_players <= 10)
            {
                Presenter.NumOfPlayerClick(No_of_players);
            }
            else { block.Text += "\nUser input: " + input + "\nInvalid input! Please enter again."; }
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            string input = box.Text;
            box.Text = "";
            if (stage == -2) // Initial Play
            {
                button.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD"));
                Presenter.PlayButtonClick();
            } 
            else if (stage == -1)     //input no of player
            {
                Stage1(input);
            }
            else if (stage == 0)
            {
                Presenter.Stage0();
            }
            else if (stage == 1)     //input name of player
            {
                Presenter.Stage1(input);
            } 
            else if (stage == 2)   //select players
            {
                Presenter.Stage2(input);
            }
    }

        private void box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            button.IsEnabled = true;
        }

        private void readonlybox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            scrollViewer.ScrollToEnd();
        }
    }
}


