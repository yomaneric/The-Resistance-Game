using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Project2
{
    public static class Presenter
    {
        static MainWindow pc;
        static MainWindow_Phone phone;
        static Console console;
        static No_of_Player NOP;
        static No_of_players_phone NOP_phone;
        static GameBoard gameBoard;
        static GameBoard_Phone gameBoard_phone;
        static Game game;
        public static int Stage = 0; //Stage 0: Start game; Stage 1: Select Player; Stage 2: Vote; Stage 3: Turn in success or fail;; 
        public static void CloseAllWindow()
        {
            pc.Close();
            phone.Close();
            console.Close();
            Environment.Exit(1);
        }
        public static string GetResultName(int i)
        {
            return game.GameResultName[i];
        }

        public static int GetResultFail(int i)
        {
            return game.GameResultFail[i];
        }

        public static void GetWindow(MainWindow Pc, MainWindow_Phone Phone, Console Console, Game newGame)
        {   
            pc = Pc;
            phone = Phone;
            console = Console;
            game = newGame;
        }

        public static void PlayButtonClick() {
            game.MissionSetUp();
            //pc view
            pc.Red.Visibility = Visibility.Hidden;
            pc.Blue.Visibility = Visibility.Hidden;
            NOP = new No_of_Player();
            pc.Main.Content = NOP;
            pc.Play.Visibility = Visibility.Hidden;
            pc.WelcomeMessage.Visibility = Visibility.Hidden;
            //phone view
            phone.Red.Visibility = Visibility.Hidden;
            phone.Blue.Visibility = Visibility.Hidden;
            NOP_phone = new No_of_players_phone();
            phone.Main.Content = NOP_phone;
            phone.Play.Visibility = Visibility.Hidden;
            phone.WelcomeMessage.Visibility = Visibility.Hidden;
            //console view
            console.block.Text += "\nHow many players? (5-10): ";
            console.button.Content = "Confirm";
            console.box.Visibility = Visibility.Visible;
            console.stage++;
        }

        public static void NumOfPlayerClick(int No_of_players)
        {
            NOP.Grid2.Visibility = Visibility.Hidden;
            NOP_phone.Grid2.Visibility = Visibility.Hidden;
            console.block.Text += "\nUser input: " + No_of_players;
            game.SetNoOfPlayers(No_of_players); //Initialized Number Of Players
            for (int i = 0; i < No_of_players; i++)
            {
                InputPlayerName inputPlayerName = new InputPlayerName(i, game);
                inputPlayerName.ShowDialog();
                if (game.playerlist.Count != i + 1)
                    i--;
            }
            // pc view
            gameBoard = new GameBoard(); 
            NOP.NoOfPlayer.Content = gameBoard;
            gameBoard.InitButton(No_of_players, game.playerlist);
            // mobile view
            gameBoard_phone = new GameBoard_Phone(); 
            gameBoard_phone.InitButton(No_of_players, game.playerlist);
            NOP_phone.NoOfPlayer.Content = gameBoard_phone;
            // console view
            console.button.Content = "Start"; 
            console.block.Text += "\nClick Start to play!";
            console.box.Text = "";
            console.button.IsEnabled = true;
            console.stage++;
        }


        public static void Stage0()
        {
            gameBoard.ResetAllButton();
            gameBoard_phone.ResetAllButton();
            game.SelectedPlayers = new List<int>();
            game.ShowRound(gameBoard.Round, gameBoard_phone.Round);
            gameBoard.Confirm.Content = "Confirm";
            gameBoard_phone.Confirm.Content = "Confirm";
            for (int i = 0; i < game.GetNoOfPlayers(); i++)
            {
                gameBoard.CheckBoxes[i].Visibility = Visibility.Visible;
                gameBoard_phone.CheckBoxes[i].Visibility = Visibility.Visible;
            }
            console.block.Text += String.Format("\n-------Round {0}-------", game.CurrentRound);
            game.StageZero(gameBoard.StatusUpdate, gameBoard_phone.StatusUpdate, console.block);
            console.button.Content = "Enter";
            console.stage++;
            Stage++;
            
        }

        public static void Stage1(string input)
        {
            string[] names = new string[100];
            int NumberToBeSelected = game.GetMission(game.GetNoOfPlayers());
            bool flag = false;
            int playerPos = -1;
            console.block.Text += "\nUser Input: " + input;
            names = input.Split();
            if (names.Length != NumberToBeSelected) // # of user input not equal to # of player to be selected
                console.block.Text += String.Format("\nPlease select {0} players!", game.GetMission(game.GetNoOfPlayers()));
            else if (names.Length != names.Distinct().Count()) // Check if duplicated names inputted
                console.block.Text += "\nYou cannot input duplicated names!";
            else
            { 
                for (int i = 0; i < NumberToBeSelected; i++)
                {
                    flag = false;
                    foreach (Player p in game.playerlist) // Compare to playerlist 
                        if (p.name == names[i]) { 
                            flag = true;
                            playerPos = p.position; // record the position of player to be added to game.SelectedPlayers
                            break;
                        }
                    if (!flag)
                        break;
                    else
                    {
                        if (playerPos != -1)
                            game.SelectedPlayers.Add(playerPos);
                        playerPos = -1;
                    }
                }
                if (flag) // All names are valid
                {
                    console.block.Text += "\nPlayer selected: "+ input + "\nPlease input all players' vote record respectively";
                    Stage++;
                    console.stage++;
                    game.StageOne(gameBoard.StatusUpdate, gameBoard_phone.StatusUpdate);
                    for (int i = 0; i < game.GetNoOfPlayers(); i++)
                    {
                        gameBoard.CheckBoxes[i].Visibility = Visibility.Hidden;
                        gameBoard.YesRadioButtons[i].Visibility = Visibility.Visible;
                        gameBoard.NoRadioButtons[i].Visibility = Visibility.Visible;
                        gameBoard_phone.CheckBoxes[i].Visibility = Visibility.Hidden;
                        gameBoard_phone.YesRadioButtons[i].Visibility = Visibility.Visible;
                        gameBoard_phone.NoRadioButtons[i].Visibility = Visibility.Visible;
                    }
                } else // Invalid names exist
                {
                    console.block.Text += "\nInvalid names! Please input again!";
                    game.SelectedPlayers.Clear();
                }
            }
        }
        public static void Stage1(List<CheckBox> cbs)
        {
            int counter = 0;
            for (int i = 0; i < cbs.Count(); i++)
                if (cbs[i].IsChecked == true)
                {
                    counter++;
                    game.SelectedPlayers.Add(i);
                }
            if (!game.PeopleNumIsMatched(counter))
            {
                MessageBox.Show(String.Format("Please select {0} players!", game.GetMission(game.GetNoOfPlayers())), "Attention!", MessageBoxButton.OK);
                game.SelectedPlayers.Clear();
            }
            else
            {
                Stage++;
                console.stage++;
                game.StageOne(gameBoard.StatusUpdate, gameBoard_phone.StatusUpdate);
                string names = "";
                foreach (int i in game.SelectedPlayers)
                    names += game.playerlist[i].name + " ";
                console.block.Text += "\nPlayer selected: " + names + "\nPlease input all players' vote record respectively";
                for (int i = 0; i < game.GetNoOfPlayers(); i++)
                {
                    gameBoard.CheckBoxes[i].Visibility = Visibility.Hidden;                   
                    gameBoard.YesRadioButtons[i].Visibility = Visibility.Visible;
                    gameBoard.NoRadioButtons[i].Visibility = Visibility.Visible;
                    gameBoard_phone.CheckBoxes[i].Visibility = Visibility.Hidden;
                    gameBoard_phone.YesRadioButtons[i].Visibility = Visibility.Visible;
                    gameBoard_phone.NoRadioButtons[i].Visibility = Visibility.Visible;
                }
            }
        }

        public static void Stage2(string input)
        {
            string[] token = new string[100];
            bool flag = true;
            int VoteNoCount = 0;
            token = input.Split();
            console.block.Text += "\nUser input: " + input;
            if (token.Length != game.GetNoOfPlayers())
            {
                console.block.Text += String.Format("\nPlease cast exactly {0} votes", game.GetNoOfPlayers());
            } else
            {
                foreach (string str in token)
                {
                    if (str == "Yes" || str == "No")
                    {
                        if (str == "No")
                            VoteNoCount++;
                    } else
                    {
                        flag = false;
                        console.block.Text += "\nInvalid vote Record. Please vote either \"Yes\" or \"No\" ";
                        break;
                    }
                }
                if (flag)
                {
                    for (int i = 0; i < game.GetNoOfPlayers(); i++)
                    {
                        gameBoard.YesRadioButtons[i].Visibility = Visibility.Hidden;
                        gameBoard.NoRadioButtons[i].Visibility = Visibility.Hidden;
                        gameBoard_phone.YesRadioButtons[i].Visibility = Visibility.Hidden;
                        gameBoard_phone.NoRadioButtons[i].Visibility = Visibility.Hidden;
                    }
                    int status = game.StageTwo(gameBoard.StatusUpdate, gameBoard_phone.StatusUpdate, VoteNoCount);
                    if (status == -1) // Fail for five consecutive time
                    {
                        MessageBox.Show(String.Format("Spy wins the game!\nThe Spies are: {0}\n\nExit?", game.Spies), "Game Ended");
                        console.block.Text += "\n" + String.Format("Spy wins the game!\nThe Spies are: {0}", game.Spies);
                        CloseAllWindow();
                    }
                    else if (status == 3)
                    { // Go to Stage 3
                        MessageBox.Show("The proposal is accepted", "Proposal Result", MessageBoxButton.OK);
                        console.block.Text += "\n" + String.Format("The proposal is accepted");
                        Stage++;
                        console.stage++;
                        console.block.Text += "\n---Turn in Success or Fail---";
                        Stage3();
                    }
                    else if (status == 0) //Fall Back to Stage 0
                    {
                        MessageBox.Show(String.Format("The proposal is rejected ({0})", game.CountFailMission), "Proposal Result", MessageBoxButton.OK);
                        console.block.Text += "\n" + String.Format("The proposal is rejected ({0})", game.CountFailMission);
                        Stage = 0;
                        console.stage = 0;
                        Stage0();
                    }
                } 
            }
                   
        }
        public static void Stage2(List<RadioButton> YesRadioButtons, List<RadioButton> NoRadioButtons)
        {
            int VoteNoCount = 0, totalVote = 0;
            foreach (RadioButton r in NoRadioButtons)
                if (r.IsChecked == true)
                    VoteNoCount++;
            foreach (RadioButton r in YesRadioButtons)
                if (r.IsChecked == true)
                    totalVote++;
            totalVote += VoteNoCount;
            if (totalVote != game.playerlist.Count)
            {
                MessageBox.Show("All players should vote!", "Attention!");
            }
            else
            {
                for (int i = 0; i < game.GetNoOfPlayers(); i++)
                {
                    gameBoard.YesRadioButtons[i].Visibility = Visibility.Hidden;
                    gameBoard.NoRadioButtons[i].Visibility = Visibility.Hidden;
                    gameBoard_phone.YesRadioButtons[i].Visibility = Visibility.Hidden;
                    gameBoard_phone.NoRadioButtons[i].Visibility = Visibility.Hidden;
                }
                int status = game.StageTwo(gameBoard.StatusUpdate, gameBoard_phone.StatusUpdate, VoteNoCount);
                if (status == -1) // Fail for five consecutive time
                { 
                    MessageBox.Show(String.Format("Spy wins the game!\nThe Spies are: {0}\n\nExit?", game.Spies), "Game Ended");
                    console.block.Text += "\n" + String.Format("Spy wins the game!\nThe Spies are: {0}", game.Spies);
                    CloseAllWindow();
                }
                else if (status == 3) { // Go to Stage 3
                    MessageBox.Show("The proposal is accepted", "Proposal Result", MessageBoxButton.OK);
                    console.block.Text += "\n" + String.Format("The proposal is accepted");
                    console.block.Text += "\n---Turn in Success or Fail---";
                    Stage++;
                    console.stage++;
                    Stage3();
                }
                else if (status == 0) //Fall Back to Stage 0
                {
                    MessageBox.Show(String.Format("The proposal is rejected ({0})", game.CountFailMission), "Proposal Result", MessageBoxButton.OK);
                    console.block.Text += "\n" + String.Format("The proposal is rejected ({0})", game.CountFailMission);
                    Stage = 0;
                    console.stage = 0;
                    Stage0();
                }
            }
        }

        public static void Stage3()
        {
            int counter = 0;
            foreach (int i in game.SelectedPlayers)
            {
                counter++;
                do
                {
                    SuccessFail TurnInWindow = new SuccessFail(i, game);
                    TurnInWindow.ShowDialog();
                } while (game.SuccessCount + game.FailCount != counter);
            }
            bool spyWin = game.StageThree(gameBoard.StatusUpdate, gameBoard_phone.StatusUpdate);
            if (spyWin)
            {
                console.block.Text += String.Format("\nThere are {0} Fail(s) in the mission. Spy wins this round!", game.FailCount);
                console.block.Text += String.Format("\nResistance {0} : {1} Spy", game.resistanceWin, game.spyWin);
                MessageBox.Show(String.Format("There are {0} Fail(s) in this mission. Spy wins this round!", game.FailCount));
                gameBoard.Images[game.CurrentRound - 1].Source = new BitmapImage(new Uri("Images/Spy.png", UriKind.Relative));
                gameBoard_phone.Images[game.CurrentRound - 1].Source = new BitmapImage(new Uri("Images/Spy.png", UriKind.Relative));
            }
            else
            {
                console.block.Text += String.Format("\nThere are {0} Fail(s) in the mission. Resistance wins this round!", game.FailCount);
                console.block.Text += String.Format("\nResistance {0} : {1} Spy", game.resistanceWin, game.spyWin);
                MessageBox.Show(String.Format("There are {0} Fail(s) in this mission. Resistance wins this round!", game.FailCount));
                gameBoard.Images[game.CurrentRound - 1].Source = new BitmapImage(new Uri("Images/Resistance.png", UriKind.Relative));
                gameBoard_phone.Images[game.CurrentRound - 1].Source = new BitmapImage(new Uri("Images/Resistance.png", UriKind.Relative));
            }
            string FinalWin = game.CheckEnd();
            if (FinalWin == "R")
            {
                console.block.Text += String.Format("\nResistance wins the game!\nThe Spies are: {0}", game.Spies);
                MessageBox.Show(String.Format("Resistance wins the game!\nThe Spies are: {0}\n\nExit?", game.Spies), "Game Ended");
                CloseAllWindow();
            }
            else if (FinalWin == "S")
            {
                console.block.Text += String.Format("\nSpy wins the game!\nThe Spies are: {0}", game.Spies);
                MessageBox.Show(String.Format("Spy wins the game!\nThe Spies are: {0}\n\nExit?", game.Spies), "Game Ended");
                CloseAllWindow();
            }
            else { 
            Stage = 0;
            console.stage = 0;
            Stage0(); // return to stage 0 for next mission
            }
        }
    }
}
