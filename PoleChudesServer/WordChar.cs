using System.ComponentModel;

namespace PoleChudesServer
{
    public class WordChar
    {
        private bool opened = false;

        public string Char { get; set; }
        public bool Opened
        {
            get => opened;
            set
            {
                opened = value;
            }
        }
    }
}
