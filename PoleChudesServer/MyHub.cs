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
            rooms.SetStart(async (p1, p2, p3, p4, id) => {
                await clientsByNickname[p1].SendAsync("opponent", rooms.players, id);
                await clientsByNickname[p2].SendAsync("opponent", rooms.players, id);
                await clientsByNickname[p3].SendAsync("opponent", rooms.players, id);
                await clientsByNickname[p4].SendAsync("opponent", rooms.players, id);
                //await clientsByNickname[p1].SendAsync("maketurn", "x");
            });
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
