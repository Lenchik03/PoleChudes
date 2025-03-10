using Microsoft.AspNetCore.SignalR;

namespace PoleChudesServer
{
    public class MyHub : Hub
    {
        private readonly Rooms rooms;
        static Dictionary<string, ISingleClientProxy> clientsByNickname = new();
        public MyHub(Rooms rooms)
        {
           this.rooms = rooms;
            rooms.SetStart(async (p1, p2, p3, p4, id) => {
                await clientsByNickname[p1].SendAsync("opponent", p1, id);
                await clientsByNickname[p2].SendAsync("opponent", p2, id);
                await clientsByNickname[p3].SendAsync("opponent", p3, id);
                await clientsByNickname[p4].SendAsync("opponent", p4, id);
                //await clientsByNickname[p1].SendAsync("maketurn", "x");
            });
        }
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("hello", "Придумай ник");
            Console.WriteLine("Новенький");
            return base.OnConnectedAsync();
        }
    }
}
