namespace JSCrunch.Core.Events
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