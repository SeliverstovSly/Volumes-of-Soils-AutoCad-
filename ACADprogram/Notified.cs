using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ACADprogram
{
    public abstract class Notified : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}