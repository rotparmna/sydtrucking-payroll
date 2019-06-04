namespace sydtrucking_payroll_front.util
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class CustomIEnumerable
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            var col = new ObservableCollection<T>();
            foreach (var cur in enumerable)
            {
                col.Add(cur);
            }
            return col;
        }
    }
}
