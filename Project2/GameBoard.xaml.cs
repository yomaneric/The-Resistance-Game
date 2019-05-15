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
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Page
    {
        public CheckBox[] AllCheckBoxes;
        public RadioButton[] AllYesRadioButtons, AllNoRadioButtons;
        public TextBlock[] AllNames;
        public Image[] Images;
        public List<CheckBox> CheckBoxes = new List<CheckBox>();
        public List<RadioButton> YesRadioButtons = new List<RadioButton>(), NoRadioButtons = new List<RadioButton>();
        public List<TextBlock> Names = new List<TextBlock>();
        

        public void StatusUpdate(string msg)
        {
            Status.Text = msg;
        }

        public GameBoard()
        {
            InitializeComponent();
            AllCheckBoxes = new CheckBox[10] { P1_Select, P2_Select, P3_Select, P4_Select, P5_Select, P6_Select, P7_Select, P8_Select, P9_Select, P10_Select };
            AllYesRadioButtons = new RadioButton[10] { P1_Yes, P2_Yes, P3_Yes, P4_Yes, P5_Yes, P6_Yes, P7_Yes, P8_Yes, P9_Yes, P10_Yes };
            AllNoRadioButtons = new RadioButton[10] { P1_No, P2_No, P3_No, P4_No, P5_No, P6_No, P7_No, P8_No, P9_No, P10_No };
            AllNames = new TextBlock[10] { P1_Name, P2_Name, P3_Name, P4_Name, P5_Name, P6_Name, P7_Name, P8_Name, P9_Name, P10_Name };
            Images = new Image[5] { missionOne, missionTwo, missionThree, missionFour, missionFive };

            foreach (RadioButton r in AllYesRadioButtons)
                r.Visibility = Visibility.Hidden;

            foreach (RadioButton r in AllNoRadioButtons)
                r.Visibility = Visibility.Hidden;

            foreach (CheckBox c in AllCheckBoxes)
                c.Visibility = Visibility.Hidden;      

            Confirm.Content = "Start";
        }

        public void InitButton(int NOP, List<Player> playerlist)
        {
            for (int i = NOP; i < 10; i++)
            {
                AllNames[i].Visibility = Visibility.Hidden;
            }
            for (int i = 0; i < NOP; i++)
            {
                CheckBoxes.Add(AllCheckBoxes[i]);
                YesRadioButtons.Add(AllYesRadioButtons[i]);
                NoRadioButtons.Add(AllNoRadioButtons[i]);
                Names.Add(AllNames[i]);
                Names[i].Text = playerlist[i].name;
            }
        }

        public void ResetAllButton()
        {
            foreach (RadioButton r in YesRadioButtons)
                r.IsChecked = false;
            foreach (RadioButton r in NoRadioButtons)
                r.IsChecked = false;
            foreach (CheckBox c in CheckBoxes)
                c.IsChecked = false;
        }


        private void mission_MouseEnter(object sender, MouseEventArgs e)
        {
            result1.Content = String.Format("Round 1 Result:\nSelected Players: {0}\nNumber of Fail: {1}", Presenter.GetResultName(0), Presenter.GetResultFail(0));
            result2.Content = String.Format("Round 2 Result:\nSelected Players: {0}\nNumber of Fail: {1}", Presenter.GetResultName(1), Presenter.GetResultFail(1));
            result3.Content = String.Format("Round 3 Result:\nSelected Players: {0}\nNumber of Fail: {1}", Presenter.GetResultName(2), Presenter.GetResultFail(2));
            result4.Content = String.Format("Round 4 Result:\nSelected Players: {0}\nNumber of Fail: {1}", Presenter.GetResultName(3), Presenter.GetResultFail(3));
            result5.Content = String.Format("Round 5 Result:\nSelected Players: {0}\nNumber of Fail: {1}", Presenter.GetResultName(4), Presenter.GetResultFail(4));
        }


        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (Presenter.Stage == 0) //Setup for Missionleader to Select Players (To stage 1)
            {
                Presenter.Stage0();
            } else if (Presenter.Stage == 1) //Selected Players Complete -> Setup Vote Session (To stage 2)
            {
                Presenter.Stage1(CheckBoxes);
            } else if (Presenter.Stage == 2) //Vote Complete -> 1. Success or Fail (To stage 3) 2. Fail (Fallback to stage 0);
            {
                Presenter.Stage2(YesRadioButtons, NoRadioButtons);
            }
        }
    }
}
