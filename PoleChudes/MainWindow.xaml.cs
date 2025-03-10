using System.ComponentModel;
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

        public List<WordChar> Word { get; set; }
        public string Variant { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Word = new List<WordChar> {
                new WordChar{ Char = "Ч" },
                new WordChar{ Char = "Ё" },
                new WordChar{ Char = "Р" },
                new WordChar{ Char = "Т" }
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void SayChar(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Variant) || Variant.Length > 1)
                return;
            string test = Variant.ToUpper();
            foreach (var wordChar in Word)
            {
                if (!wordChar.Opened && wordChar.Char == test)
                {
                    var anim = FindResource("Storyboard1") as Storyboard;
                    anim.Begin();
                    Task.Delay(1000).ContinueWith(s =>
                    {
                        wordChar.Opened = true;
                    });
                }
            }
        }
    }
}