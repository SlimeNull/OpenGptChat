using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat.Models
{
    public partial class ValueWrapper<T> : ObservableObject
    {
        public ValueWrapper(T value)
        {
            Value = value;
        }

        [ObservableProperty]
        private T _value;
    }
}
