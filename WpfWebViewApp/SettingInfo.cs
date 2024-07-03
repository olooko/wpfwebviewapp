using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWebViewApp
{
    public class SettingInfo
    {
        public Uri Source { get; }

        public int Position { get; }

        public SettingInfo(Uri source, int position)
        {
            this.Source = source;
            this.Position = position;
        }
    }
}
