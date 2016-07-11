using System;

namespace JSCrunch.VisualStudio
{
    public class ProcessingItem
    {
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
    }
}