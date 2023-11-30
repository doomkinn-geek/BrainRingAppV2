using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BrainRingAppV2.ViewModels
{
    public class ButtonViewModel : INotifyPropertyChanged
    {
        private int _buttonId;
        private Visibility _visibility;
        private Brush _background;
        private string _text;
        private int _pressOrder;
        private double _pressTime;

        public int ButtonId
        {
            get => _buttonId;
            set => SetField(ref _buttonId, value);
        }

        public Visibility Visibility
        {
            get => _visibility;
            set => SetField(ref _visibility, value);
        }

        public Brush Background
        {
            get => _background;
            set => SetField(ref _background, value);
        }

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public int PressOrder 
        {
            get => _pressOrder;
            set => SetField(ref _pressOrder, value);
        }
        public double PressTime 
        {
            get => Math.Round(_pressTime/1000, 2);
            set => SetField(ref _pressTime, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
