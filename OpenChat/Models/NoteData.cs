using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChat.Models
{
    public class NoteData : INotifyPropertyChanged
    {
        public string Text { get; set; } = string.Empty;

        public bool Show { get; set; } = false;


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
