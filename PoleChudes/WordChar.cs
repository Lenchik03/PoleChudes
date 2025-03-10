using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoleChudes
{
    public class WordChar : INotifyPropertyChanged
    {
        private bool opened = false;

        public string Char { get; set; }
        public bool Opened
        {
            get => opened;
            set
            {
                opened = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Opened)));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;


    }
}
