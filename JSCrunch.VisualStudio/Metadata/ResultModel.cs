using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JSCrunch.VisualStudio.Metadata
{
    public class ResultModel
    {
        public string Name { get; set; }

        public Uri Icon
        {
            get
            {
                var iconToUse = Success ? "success.png" : "failure.png";

                return new Uri("pack://application:,,,/JSCrunch.VisualStudio;component/Resources/" + iconToUse);
            }
        }

        public bool Success { get; set; }
    }
}