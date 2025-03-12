using Microsoft.AspNetCore.SignalR;

namespace PoleChudesServer
{
    public class Rooms
    {
        string last = string.Empty;
        public List<string> players = new();
        public List<WordChar> letters = new();

        internal void AddNewClient(string nickname)
        {
            if (players.Count <3)
                players.Add(nickname);
            else
            {
                players.Add(nickname);
                StartNewGame(players[0], players[1], players[2], players[3]);
            }
        }

        private MyHub myHub;
        public void SetHub(MyHub myHub)
        {
            this.myHub = myHub;
        }

        Dictionary<string, Game> games = new();
        private void StartNewGame(string p1, string p2, string p3, string p4)
        {
            string vopros = "колонка для одного";
            string word = "НАУШНИКИ";
            letters = word.Select(x => new WordChar { Char = x.ToString()}).ToList();
            var game = new Game { ID = Guid.NewGuid().ToString(), P1 = p1, P2 = p2, P3 = p3, P4 = p4 };
            games.Add(game.ID, game);

            myHub.Clients.All.SendAsync("opponent", players, game.ID);
            myHub.Clients.All.SendAsync("start", game.ID, vopros, letters);
            MyHub.clientsByNickname[players[0]].SendAsync("maketurn", "Буква ...");
        }

        int index = 0;
        

        internal void ClearGame(string gameId)
        {
            games.Remove(gameId);
        }

        int correctLetters = 0;
        internal void CheckTurn(string nickName, string letter)
        {
            string result = string.Empty;
            int count = 0;
            //var letter1 = letters.FirstOrDefault(s => s.Char == letter);
            if (letter.Length > 1)
            {
                bool fullword = string.Join("", letters.Select(s => s.Char)).ToLower() == letter.ToLower();
                if (fullword)
                {
                    letters.ForEach(s => s.Opened = true);
                    myHub.Clients.All.SendAsync("update", letters);
                    myHub.Clients.All.SendAsync("winner", players[index]);
                    return;
                }
            }
            else
            {
                foreach (var let in letters)
                {
                    if (letter == let.Char)
                    {
                        count++;
                        correctLetters++;
                        let.Opened = true;
                        result = players[index];
                        myHub.Clients.All.SendAsync("update", letters);
                    }

                }
            }
            

            if (count == 0)
            {
                myHub.Clients.All.SendAsync("loser", letter);
                ++index;
                if (index > 3)
                    index = 0;
                result = players[index];
            }
            MyHub.clientsByNickname[result].SendAsync("maketurn", "Буква ...");
            
            if (correctLetters == letters.Count)
                myHub.Clients.All.SendAsync("winner", players[index]);
        }
    }
}
