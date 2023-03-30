using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat_MAUI.Utilities
{
    public partial class ValueWrapper<T> : ObservableObject
    {
        public ValueWrapper(T value)
        {
            _value = value;
        }

        [ObservableProperty]
        private T _value;
    }
}
