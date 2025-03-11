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
            if (players.Count <4)
                players.Add(nickname);
            else
            {
                StartNewGame(players[0], players[1], players[2], players[3]);
            }
        }
        Action<string, string, string,string, string> proc;
        internal void SetStart(Action<string, string, string, string, string> proc)
        {
            this.proc = proc;
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
            string word = "наушники";
            letters = word.Select(x => new WordChar { Char = x.ToString()}).ToList();
            var game = new Game { ID = Guid.NewGuid().ToString(), P1 = p1, P2 = p2, P3 = p3, P4 = p4 };
            myHub.Clients.All.SendAsync("start", game, word, letters);
            MyHub.clientsByNickname[players[0]].SendAsync("Буква ...");

            games.Add(game.ID, game);
            proc(p1, p2, p3, p4, game.ID);
        }

        int index = 0;
        internal string GetNextPlayer(Turn turn)
        {
            string result = string.Empty;
            string letter = letters.FirstOrDefault(s => s.Char == turn.Letter).ToString();

            if (letter != null)
                result = players[index];
            else
            {
                result = players[index++];
            }
            
            return null;
        }

        internal void ClearGame(string gameId)
        {
            games.Remove(gameId);
        }
    }
}
