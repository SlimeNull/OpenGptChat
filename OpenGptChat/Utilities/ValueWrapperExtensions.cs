using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenGptChat.Models;

namespace OpenGptChat.Utilities
{
    public static class ValueWrapperExtensions
    {
        public static ValueWrapper<T> Wrap<T>(this T item) => new ValueWrapper<T>(item);

        public static ObservableCollection<ValueWrapper<T>> WrapCollection<T>(this IEnumerable<T> collection)
        {
            ObservableCollection<ValueWrapper<T>> wrapped =
                new ObservableCollection<ValueWrapper<T>>();

            foreach (var item in collection)
                wrapped.Add(item.Wrap());

            return wrapped;
        }

        public static void UnwrapTo<T>(this ObservableCollection<ValueWrapper<T>> wrapped, IList<T> list)
        {
            list.Clear();

            foreach (var item in wrapped)
                list.Add(item.Value);
        }

        public static T[] UnwrapToArray<T>(this ObservableCollection<ValueWrapper<T>> wrapped)
        {
            return wrapped
                .Select(wrapped => wrapped.Value)
                .ToArray();
        }
    }
} 
