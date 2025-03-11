using Microsoft.AspNetCore.SignalR;

namespace PoleChudesServer
{
    public class MyHub : Hub
    {
        private readonly Rooms rooms;
        public static Dictionary<string, ISingleClientProxy> clientsByNickname = new();
        public MyHub(Rooms rooms)
        {
           this.rooms = rooms;
            rooms.SetHub(this);
        }
        public override Task OnConnectedAsync()
        {
            if(rooms.players.Count == 4)
            {
                Context.Abort();
            }
            Clients.Caller.SendAsync("hello", "Придумай ник");
            Console.WriteLine("Новенький");
            return base.OnConnectedAsync();
        }
        public void MakeTurn(string nickName, string letter)
        {
            rooms.CheckTurn(nickName, letter);
        }
         
        public void Nickname(string nickname)
        {
            var check = clientsByNickname.Keys.FirstOrDefault(s => s == nickname);
            if (check != null)
            {
                Clients.Caller.SendAsync("hello", "Придумай другой ник");
                return;
            }
            else
            {
                clientsByNickname.Add(nickname, Clients.Caller);
                rooms.AddNewClient(nickname);
            }
        }
    }
}
