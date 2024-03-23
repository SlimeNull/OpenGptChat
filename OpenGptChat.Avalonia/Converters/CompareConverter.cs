using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Data.Converters;

namespace OpenGptChat.Converters
{
    public class CompareConverter : IValueConverter
    {
        public enum ValueComparison
        {
            Equal, NotEqual
        }

        public ValueComparison Comparison { get; set; }
        public object? TargetValue { get; set; }

        public bool Compare(object? value)
        {
            if (Comparison == ValueComparison.Equal)
                return Object.Equals(value, TargetValue);
            else if (Comparison == ValueComparison.NotEqual)
                return !Object.Equals(value, TargetValue);

            return false;
        }


        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Compare(value);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException(); 
        }
    }
}
