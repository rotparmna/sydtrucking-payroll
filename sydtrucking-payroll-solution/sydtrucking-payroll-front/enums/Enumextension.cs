namespace sydtrucking_payroll_front.enums
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumExtension
    {
        public static IEnumerable<T> Get<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
