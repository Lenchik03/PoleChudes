using Microsoft.AspNetCore.SignalR;

namespace PoleChudesServer
{
    public class Rooms
    {
        string last = string.Empty;
        List<string> players = new(); 

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
            List<WordChar> letters = word.Select(x => new WordChar { Char = x.ToString()}).ToList();
            var game = new Game { ID = Guid.NewGuid().ToString(), P1 = p1, P2 = p2, P3 = p3, P4 = p4, Turn = p1 };
            myHub.Clients.All.SendAsync("start", game, word, letters);
            games.Add(game.ID, game);
            proc(p1, p2, p3, p4, game.ID);
        }

        internal string GetNextPlayer(Turn turn)
        {

            return null;
        }




        internal void ClearGame(string gameId)
        {
            games.Remove(gameId);
        }
    }
}
