using SourceMapDotNet.Model;

namespace JSCrunch
{
    public class SourceLocation
    {
        public string What { get; set; }
        public string Where { get; set; }

        public SourceReference Position { get; set; }

        public override string ToString()
        {
            var what = string.IsNullOrEmpty(What) ? "(unknown)" : What;

            if (Where == null)
            {
                return what;
            }

            return $"{what} at {Position.File}:{Position.LineNumber}";
        }
    }
}