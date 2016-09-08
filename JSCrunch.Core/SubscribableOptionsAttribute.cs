using System;

namespace JSCrunch.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SubscribableOptionsAttribute : Attribute
    {
        public bool LoadDeferred { get; set; }
    }
}