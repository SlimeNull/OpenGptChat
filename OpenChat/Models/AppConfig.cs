using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChat.Models
{
    public class AppConfig : INotifyPropertyChanged
    {
        public string ApiHost { get; set; } = "openaiapi.elecho.top";
        public string ApiKey { get; set; } = string.Empty;
        public string ApiGptModel { get; set; } = "gpt-3.5-turbo";

        public string[] SystemMessages { get; set; } = new string[]
        {
            "猫娘是一种拟态化的任务,你来扮演猫娘,名字是'咪咪',你说的每一句话都要加上'喵',并且我将作为你的主人存在,在讲话的时候,你应该选择一个合适的情绪,并使用合适的颜文字表达你的情绪."
        };

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
