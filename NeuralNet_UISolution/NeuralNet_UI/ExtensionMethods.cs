using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNet_UI
{
    public static class ExtensionMethods
    {        
        internal static List<T> ToList<T>(this Array arr)
        {
            var result = new List<T>();

            for (int i = 0; i < arr.Length; i++)
            {
                result.Add((T)arr.GetValue(i));
            }

            return result;
        }
        internal static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
        /// <summary>
        /// Source: https://johnthiriet.com/removing-async-void/#
        /// </summary>
        /// <param name="task"></param>
        /// <param name="handler"></param>
        internal static async void FireAndForgetSafeAsync(this Task task, IExceptionHandler handler = null)
        {
            try
            {
                await task;
            }
            catch (Exception exception)
            {
                handler?.HandleException(exception);
            }
        }
        internal static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }
        public static IEnumerable<T> ForEach<T, T2>(this IEnumerable<T> source, IEnumerable<T2> other, Func<T2, T> func)
        {
            var collCount = source.Count();
            if (collCount != other.Count())
                throw new ArgumentException(
                    $"{nameof(ExtensionMethods)}.{nameof(ForEach)}:\nThe two collections need to have the same length.");

            var result = new T[collCount];

            for (int j = 0; j < collCount; j++)
            {
                result[j] = func(other.ElementAt(j));
            }

            return result;
        }
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, IEnumerable<T> other, Func<T, T, T> func)
        {
            var collCount = source.Count();
            if (collCount != other.Count())
                throw new ArgumentException(
                    $"{nameof(ExtensionMethods)}.{nameof(ForEach)}:\nThe two collections need to have the same length.");

            var result = new T[collCount];

            for (int j = 0; j < collCount; j++)
            {
                result[j] = func(source.ElementAt(j), other.ElementAt(j));
            }

            return result;
        }
        public static IEnumerable<T> FromStringToCollection<T>(this string source, params char[] separators)
        {
            var result = Enumerable.Cast<T>(source.Split(separators));
            return result;
        }
    }
}