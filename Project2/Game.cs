using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
namespace Project2
{
    public class Game
    {
        public List<Player> playerlist { get; private set; }
        public string[] GameResultName { get; private set; }
        public int[] GameResultFail { get; private set; }
        public int CurrentRound { get; private set; }
        public int FailCount { get { return failCount; } set { failCount++; } }
        public int SuccessCount { get { return successCount; } set { successCount++; } }
        public string Spies { get; private set; }
        public int resistanceWin { get; private set; }
        public int spyWin { get; private set; }

        const int rounds = 5;
        public int CountFailMission = 0;
        int LeaderPos = 0, No_of_players, failCount = 0, successCount = 0;
        Player missionLeader;
        List<int> spyIndex = new List<int>();
        public List<int> SelectedPlayers = new List<int>();
        Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
        public delegate void Callback(string msg);

        public Game()
        {
            CurrentRound = 1;
            GameResultName = new string[5];
            GameResultFail = new int[5];
            resistanceWin = 0;
            spyWin = 0;
            Spies = "";
        }

        public void MissionSetUp()
        {
            List<int> list1 = new List<int>() { 2, 3, 2, 3, 3 };
            List<int> list2 = new List<int>() { 2, 3, 4, 3, 4 };
            List<int> list3 = new List<int>() { 2, 3, 3, 4, 4 };
            List<int> list4 = new List<int>() { 3, 4, 4, 5, 5 };

            dict.Add(5, list1);
            dict.Add(6, list2);
            dict.Add(7, list3);
            dict.Add(8, list4);
            dict.Add(9, list4);
            dict.Add(10, list4);
        }

        public void ResetSuccessFail()
        {
            successCount = 0;
            failCount = 0;
        }
        public bool PeopleNumIsMatched(int counter)
        {
            if (counter == GetMission(No_of_players))
                return true;
            return false;
        }
        private void GenSpyIndex(int No_of_players, int No_of_spy)
        {
            Random rnd = new Random();
            while (spyIndex.Count < No_of_spy)
            {
                int index = rnd.Next(1, No_of_players);
                if (!spyIndex.Contains(index))
                    spyIndex.Add(index);
            }
        }

        public void SetNoOfPlayers(int No_of_players)
        {
            this.No_of_players = No_of_players;
            playerlist = new List<Player>(No_of_players);
        }

        public int GetNoOfPlayers()
        {
            return No_of_players;
        }
        public void SetMissionLeader()
        {
            missionLeader = playerlist[(LeaderPos % No_of_players)];
        }

        public void ShowRound(TextBlock textBlock, TextBlock textBlock_phone)
        {
            textBlock.Text = "Round " + CurrentRound.ToString();
            textBlock_phone.Text = "Round " + CurrentRound.ToString();
        }

        public int GetMission(int No_of_players)
        {
            return dict[No_of_players][CurrentRound - 1];
        }


        public void InputPlayer(int pos, string name, TextBlock ErrorMessage, TextBlock IdentityShow, Button Enter, Image image)
        {
            int No_of_spy = (int)Math.Ceiling((double)No_of_players / 3);
            int No_of_resistance = No_of_players - No_of_spy;
            GenSpyIndex(No_of_players, No_of_spy); // Generate Random Spy list
            string input;
            input = name;
            if (input.Length > 8) // Check if name length > 8
                ErrorMessage.Text = "Please input name with length not more than 8 letters!";
            else if (input.Length == 0) // Check if name is empty
                ErrorMessage.Text = "Input name cannot be empty!";
            else if (input.Contains(" "))
                ErrorMessage.Text = "Input name cannot contains spacebar!";
            else
            {
                bool repeated = false;
                foreach (Player p in playerlist)
                    if (p.name == input)
                    { // check if name exists
                        ErrorMessage.Text = "Please input other name! This name is used by other players!";
                        repeated = true;
                        break;
                    }
                if (!repeated)
                {
                    int identity;
                    if (spyIndex.Contains(pos))
                    {
                        identity = 0; // spy = 0
                        IdentityShow.Text = "You are Spy!";
                        IdentityShow.Foreground = new SolidColorBrush(Colors.Red);
                        Spies += name + ", ";
                        image.Source = new BitmapImage(new Uri("Images/Spy.png", UriKind.Relative));
                    }
                    else
                    {
                        identity = 1; // resistance = 1
                        IdentityShow.Text = "You are Resistance!";
                        image.Source = new BitmapImage(new Uri("Images/Resistance.png", UriKind.Relative));
                    }
                    playerlist.Add(new Player(input, pos, identity));
                    Enter.Visibility = Visibility.Hidden;
                }
            }
        }

        public void StageZero(Callback StatusUpdate, Callback StatusUpdate_phone, TextBox consoleBlock)
        {
            SetMissionLeader();
            int playerToBeSelected = GetMission(No_of_players);
            StatusUpdate(String.Format("{0}, please choose {1} players", missionLeader.name, playerToBeSelected));
            StatusUpdate_phone(String.Format("{0}, please choose {1} players", missionLeader.name, playerToBeSelected));
            string playernames = "";
            for (int i = 0; i<No_of_players; i++)
                playernames += playerlist[i].name + " ";
            consoleBlock.Text += String.Format("\n{0}, please choose {1} players\n{2}", missionLeader.name, playerToBeSelected, playernames);
        }

        public void StageOne(Callback StatusUpdate, Callback StatusUpdate_phone)
        {
            string players = "";
            foreach (int i in SelectedPlayers)
            {
                players += playerlist[i].name + ", ";
            }
            players = players.Remove(players.Length - 2, 2);
            StatusUpdate(String.Format("Please vote YES or NO to {0}'s proposal\n({1})", missionLeader.name, players));
            StatusUpdate_phone(String.Format("Please vote YES or NO to {0}'s proposal\n({1})", missionLeader.name, players));
        }

        public int StageTwo(Callback StatusUpdate, Callback StatusUpdate_phone, int VoteNoCount)
        {
            if (VoteNoCount >= (int)Math.Ceiling((double)No_of_players / 2))
            {
                CountFailMission++;
                if (CountFailMission == 5)
                {
                    StatusUpdate("Mission failed consecutively for 5 times. Spy wins!");//SPY WIN
                    StatusUpdate_phone("Mission failed consecutively for 5 times. Spy wins!");
                    Spies = Spies.Remove(Spies.Length - 2, 2);
                    return -1;
                }
                LeaderPos++;
                SetMissionLeader();
                return 0;
            }
            else
            {
                StatusUpdate("---Turn in Success of Fail---");
                StatusUpdate_phone("---Turn in Success of Fail---");
                return 3;
            }
        }

        public bool StageThree(Callback StatusUpdate, Callback StatusUpdate_phone)
        {
            int failCounter;
            if (playerlist.Count >= 7 && CurrentRound == 4) failCounter = 2;
            else failCounter = 1;
            if (FailCount >= failCounter)
            {
                spyWin++;
                StatusUpdate("Spy wins this round!");
                StatusUpdate_phone("Spy wins this round!");
                return true;
            }
            else
            {
                resistanceWin++;
                StatusUpdate("Resistance wins this round!");
                StatusUpdate_phone("Resistance wins this round!");
                return false;
            }
        }
        public string CheckEnd() { 
            if (resistanceWin == 3)
            {
                Spies = Spies.Remove(Spies.Length - 2, 2);
                return "R";
            }
            else if (spyWin == 3)
            {
                Spies = Spies.Remove(Spies.Length - 2, 2);
                return "S";
            } else {
                string playersname = "";
                foreach (int i in SelectedPlayers)
                    playersname += playerlist[i].name + " ";
                GameResultName[CurrentRound - 1] = playersname;
                GameResultFail[CurrentRound - 1] = FailCount;
                LeaderPos++;
                ResetSuccessFail();
                CurrentRound++;
                return "N";
            }
        }

    }
}




