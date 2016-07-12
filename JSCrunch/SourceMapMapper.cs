using System;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SourceMapDotNet;
using SourceMapDotNet.Model;

namespace JSCrunch
{
    public class SourceMapMapper
    {
        public static string SourceLinesFromStackTrace(string stackTrace)
        {
            var content = HttpUtility.UrlDecode(stackTrace);
            var message = string.Empty;
            var pattern = " (evaluating ";

            var pos = content.IndexOf(pattern, StringComparison.Ordinal);

            if (pos > 0)
            {
                message = content.Substring(0, pos).Trim();
                content = content.Substring(pos).Trim();
            }

            var lines = content
                .Split(new[] { "&#10;&#9;&#9;", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseLine)
                .ToArray();

            var groupedByFile = lines
                .GroupBy(l => l.Position.File, l => l)
                .Where(f => !string.IsNullOrEmpty(f.Key))
                .ToList();

            foreach (var file in groupedByFile)
            {
                var pathToSourceMap = file.Key + ".map";
                Uri parsedUri;
                if (Uri.TryCreate(pathToSourceMap, UriKind.Absolute, out parsedUri))
                {
                    pathToSourceMap = parsedUri.LocalPath;

                    if (File.Exists(pathToSourceMap))
                    {
                        Map(file.ToArray(), pathToSourceMap);
                    }
                }
            }

            // Probably the first line in the stack is the message
            if (string.IsNullOrEmpty(message) && lines.First().Where == null)
            {
                message = lines.First().What;
                lines = lines.Skip(1).ToArray();
            }

            var separator = "\n\t\t";
            return message + separator + string.Join(separator, lines.Select(line => line.ToString()));
        }

        private static SourceLocation ParseLine(string line)
        {
            var trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("at "))
            {
                SourceLocation retval;

                var parts = line.Split(new[] { " (" }, StringSplitOptions.None);
                if (parts.Length == 1)
                {
                    var where = parts[0].Trim().Replace("at ", "");
                    retval = new SourceLocation { Where = where };
                }
                else
                {
                    var what = parts[0].Trim().Replace("at ", "");
                    var where = parts[1].Trim().Replace(")", "");
                    retval = new SourceLocation { What = what, Where = where };
                }

                retval.Position = PositionFrom(retval.Where);

                return retval;
            }
            else
            {
                var parts = line.Split(new[] { " in " }, StringSplitOptions.None);

                if (parts.Length == 1)
                {
                    return new SourceLocation
                    {
                        What = line
                    };
                }

                var what = parts[0].Trim();

                what = what.Replace("(evaluating &apos;", "").Replace("&apos;)", "").Trim();

                var where = parts[1].Trim().Replace("(line ", ":").Replace(")", ":0");

                return new SourceLocation
                {
                    What = what,
                    Where = @where,
                    Position = PositionFrom(@where)
                };
            }
        }

        private static SourceReference PositionFrom(string location)
        {
            var columnPosition = location.LastIndexOf(":", StringComparison.Ordinal);

            if (columnPosition < 0)
            {
                return new SourceReference {File = location, LineNumber = 0};
            }

            var linePosition = location.LastIndexOf(":", columnPosition - 1, StringComparison.Ordinal);

            var line = int.Parse(location.Substring(linePosition + 1, columnPosition - linePosition - 1));

            return new SourceReference { LineNumber = line, File = location.Substring(0, linePosition).Trim() };
        }

        public static object Map(SourceLocation[] locations, string pathToSourceMap)
        {
            var file = JsonConvert.DeserializeObject<SourceMapFile>(File.ReadAllText(pathToSourceMap));

            var consumer = new SourceMapConsumer(file);

            foreach (var location in locations)
            {
                var mappedPositions = consumer.OriginalPositionsFor(location.Position.LineNumber);
                if (mappedPositions.Length >= 1)
                {
                    location.Position = mappedPositions.First();
                }
            }

            return null;
        }
    }
}