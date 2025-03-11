using Microsoft.AspNetCore.SignalR.Client;
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
using System.Windows.Shapes;

namespace PoleChudes
{
    /// <summary>
    /// Логика взаимодействия для NickNameWindow.xaml
    /// </summary>
    public partial class NickNameWindow : Window
    {
        private readonly HubConnection connection;

        public string NickName { get; set; }

        public NickNameWindow(HubConnection Connection)
        {
            InitializeComponent();
            DataContext = this;
            connection = Connection;
        }

        private async void Send(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NickName))
            {
                MessageBox.Show("Ник не может быть пустым");
                return;
            }
            await connection.SendAsync("Nickname", NickName);
            Close();
        }
    }
}
