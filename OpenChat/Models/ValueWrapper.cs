using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChat.Models
{
    public class ValueWrapper<T> : INotifyPropertyChanged
    {
        public ValueWrapper(T value)
        {
            Value = value;
        }

        public T Value { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
