using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BrainRingAppV2.ViewModels
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public bool Set<T>(ref T field, T value, [CallerMemberName] string prop = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(prop);
            return true;
        }
    }
}