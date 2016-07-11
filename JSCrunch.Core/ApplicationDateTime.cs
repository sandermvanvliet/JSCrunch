using System;

namespace JSCrunch.Core
{
    public class ApplicationDateTime
    {
        static ApplicationDateTime()
        {
            UtcNow = () => DateTime.UtcNow;
        }

        public static Func<DateTime> UtcNow { get; set; }
    }
}