using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancialTracker.ViewModels
{
    public class BaseWindowViewModel : INotifyPropertyChanged
    {
        public Window Window { get; private set; }
        public BaseWindowViewModel(Window window)
        {
            Window = window;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
