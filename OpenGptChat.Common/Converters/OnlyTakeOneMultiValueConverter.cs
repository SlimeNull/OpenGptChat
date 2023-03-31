using System.Globalization;
using System.Windows.Data;

namespace OpenGptChat.Converters
{
    public class OnlyTakeOneMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 0)
                throw new ArgumentException("Not enough values");

            object value = values[0];
            if (value == null)
                throw new ArgumentNullException("value");

            if (!targetType.IsAssignableFrom(value.GetType()))
                throw new ArgumentException("Not assignable");

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Type sourceType = value.GetType();

            foreach (var targetType in targetTypes)
                if (!targetType.IsAssignableFrom(sourceType))
                    throw new ArgumentException("Not assignable");

            object[] result = new object[targetTypes.Length];
            Array.Fill(result, value);

            return result;
        }
    }
}
