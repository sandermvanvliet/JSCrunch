using System;

namespace JSCrunch
{
    public class TestRequest
    {
        public DateTime Timestamp { get; }
        public string Path { get; }

        public TestRequest(DateTime timestamp, string path)
        {
            Timestamp = timestamp;
            Path = path;
        }
    }
}