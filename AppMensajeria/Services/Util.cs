using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppMensajeria.Services
{
    public static class Util
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return new ObservableCollection<T>(source);
        }
    }
}
