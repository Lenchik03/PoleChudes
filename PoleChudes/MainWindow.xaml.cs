using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PoleChudes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        HubConnection _connection;
        private List<string> opponents;
        public List<string> Opponents
        {
            get => opponents;
            set
            {
                opponents = value;
                Signal();
            }
        }
        private string nickName;

        public string NickName
        {
            get => nickName;
            set
            {
                nickName = value;
                Signal();
            }
        }
        public string Message
        {
            get => message;
            set { message = value;
                Signal();
            }
        }
        public string Question { 
            get => question;
            set { question = value; 
                Signal();
            }
        }

        string gameid = string.Empty;
        private List<WordChar> word;
        private string question;
        private bool myTurn;

        public string Address { get; set; } = "http://localhost:5010";
        public List<WordChar> Word
        {
            get => word;
            set { word = value;
                Signal();
            }
        }

        public bool MyTurn { get => myTurn;
            set { myTurn = value;
                Signal();
            }
            
        }
        string musicFilePath = "";
        private string message;

        public string Variant { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            CreateConnection();
            HubMethods();

            DataContext = this;

            //Word = new List<WordChar> {
            //    new WordChar{ Char = "Ч" },
            //    new WordChar{ Char = "Ё" },
            //    new WordChar{ Char = "Р" },
            //    new WordChar{ Char = "Т" }
            //};
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void PlayMusic(string musicFilePath)
        {
            try
            {
                //if(!string.IsNullOrEmpty(musicFilePath))
                //{
                //    mediaPlayer.Stop();
                //    Task task = new Task();

                //}
                Dispatcher.Invoke(() =>
                {
                    mediaPlayer.Open(new Uri(musicFilePath, UriKind.Relative));
                    mediaPlayer.Play();
                });
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке музыки: {ex.Message}");
            }
        }

        private void HubMethods()
        {
            _connection.On<string>("hello", s =>
            {
                Dispatcher.Invoke(() =>
                {
                    var win = new NickNameWindow(_connection);
                    win.ShowDialog();
                    NickName = win.NickName;
                });
            });

            _connection.On<List<string>, string>("opponent", (s, id) =>
            {
                Opponents = s;
                gameid = id;
            });

            _connection.On<string, string, List<WordChar>>("start", (id, vopros, letters) =>
            {
                gameid = id;
                Question = vopros;
                Word = letters;
               
            });

            _connection.On<string>("maketurn", letter =>
            {
                MyTurn = true;
                musicFilePath = "music/baraban.mp3";
                PlayMusic(musicFilePath);
                Message = letter;
            });

            _connection.On<List<WordChar>, string, string>("update", (letters, player, letter) =>
            {
                Word = letters;
                musicFilePath = "music/bukvap.mp3";
                PlayMusic(musicFilePath);
                Message = $"Игрок {player} отгадал {letter}";
            });
            _connection.On<string>("winner", player =>
            {
                MyTurn = false;
                musicFilePath = "music/winner.mp3";
                PlayMusic(musicFilePath);
                Message = $"Победил игрок - {player}";


            });
            _connection.On<string, string>("loser", (wrong, loser) =>
            {
                musicFilePath = "music/bukvan.mp3";
                PlayMusic(musicFilePath);
                Message = $"Игрок {loser} сказал {wrong} и оказался не прав";
            });

        }

        private void SayChar(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Variant))
                return;
            string test = Variant.ToUpper();
            MyTurn = false;
            _connection.SendAsync("MakeTurn", nickName, test);
           

            //foreach (var wordChar in Word)
            //{
            //    if (!wordChar.Opened && wordChar.Char == test)
            //    {
            //        var anim = FindResource("Storyboard1") as Storyboard;
            //        anim.Begin();
            //        Task.Delay(1000).ContinueWith(s =>
            //        {
            //            wordChar.Opened = true;
            //        });
            //    }
            //}
        }
        private void CreateConnection()
        {
            _connection = new HubConnectionBuilder().
                            AddJsonProtocol(s =>
                            {
                                s.PayloadSerializerOptions.ReferenceHandler =
                                System.Text.Json.Serialization.ReferenceHandler.Preserve;
                            }
                            ).
                        WithUrl(Address + "/game").
                        Build();

            _connection.StartAsync();

            Unloaded += async (s, e) => await _connection.StopAsync();
        }

        private void SetAddress(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}