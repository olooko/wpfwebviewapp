using CefSharp.DevTools.Profiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WpfWebViewApp
{
    public class Setting
    {
        public const string XML_FILE = "Setting.xml";
        public const WebViewType DEFAULT_WEBVIEW = WebViewType.Edge;
        public const string DEFAULT_URL = "https://webview.olooko.xyz";
        public const TouchPosition DEFAULT_POS = TouchPosition.TopLeft;

        public WebViewType WebViewType { get; }

        public string Url { get; }

        public TouchPosition TouchPosition { get; }

        public Setting(WebViewType webviewType, string url, TouchPosition touchPosition)
        {
            this.WebViewType = webviewType;
            this.Url = url;
            this.TouchPosition = touchPosition;
        }

        public static Setting GetInformation()
        {
            XDocument xdoc;

            if (!File.Exists(XML_FILE))
            {
                xdoc = new XDocument(
                    new XElement("setting",
                        new XElement("webview", DEFAULT_WEBVIEW),
                        new XElement("url", DEFAULT_URL),
                        new XElement("position", DEFAULT_POS)
                    )
                );
                xdoc.Save(XML_FILE);
            }
            else
                xdoc = XDocument.Load(XML_FILE);

            XElement xroot = xdoc.Root;

            if (xroot != null)
            {
                XElement webview = xroot.Element("webview") ?? new XElement("webview", DEFAULT_WEBVIEW);
                XElement url = xroot.Element("url") ?? new XElement("url", DEFAULT_URL);
                XElement position = xroot.Element("position") ?? new XElement("position", DEFAULT_POS);

                return new Setting((WebViewType)Enum.Parse(typeof(WebViewType), webview.Value), url.Value, (TouchPosition)Enum.Parse(typeof(TouchPosition), position.Value));
            }

            return new Setting(DEFAULT_WEBVIEW, DEFAULT_URL, DEFAULT_POS);
        }

        public static void Save(WebViewType webviewType, string url, TouchPosition touchPosition)
        {
            XDocument xdoc = new XDocument(
                new XElement("setting",
                    new XElement("webview", webviewType.ToString()),
                    new XElement("url", url),
                    new XElement("position", touchPosition.ToString())
                )
            );

            xdoc.Save(Setting.XML_FILE);
        }
    }
}
