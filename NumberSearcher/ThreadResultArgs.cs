using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using NumberSearcher.Annotations;

namespace NumberSearcher
{
    internal class ThreadResultArgs : EventArgs, INotifyPropertyChanged
    {
        private int result;
        private string name;
        private string dateTime;
        private LinearGradientBrush brush;

        public LinearGradientBrush Brush
        {
            get { return brush; }
            set
            {
                brush = value;
                OnPropertyChanged(nameof(Brush));
            }
        }

        public int Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string DateTime
        {
            get { return dateTime; }
            set
            {
                dateTime = value;
                OnPropertyChanged(nameof(DateTime));
            }
        }

        public ThreadResultArgs()
        {
        }

        public ThreadResultArgs(int result, string name, string dateTime)
        {
            Result = result;
            Name = name;
            DateTime = dateTime;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
