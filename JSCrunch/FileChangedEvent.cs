using JSCrunch.Core.Events;

namespace JSCrunch
{
    public class FileChangedEvent : Event
    {
        public string Path { get; }

        public FileChangedEvent(string path)
        {
            Path = path;
        }
    }
}