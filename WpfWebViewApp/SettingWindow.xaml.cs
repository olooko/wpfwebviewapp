using System;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace WpfWebViewApp
{
    public partial class SettingWindow : Window
    {
        private const string SETTING_XML = "Setting.xml";
        private const string DEFAULT_SRC = "https://erasebg.cafe24.com";
        private const string DEFAULT_POS = "0";

        public static SettingInfo GetInformation()
        {
            XDocument xdoc;

            if (!File.Exists(SETTING_XML))
            {
                xdoc = new XDocument(
                    new XElement("setting",
                        new XElement("source", DEFAULT_SRC),
                        new XElement("position", DEFAULT_POS)
                    )
                );
                xdoc.Save(SETTING_XML);
            }
            else
                xdoc = XDocument.Load(SETTING_XML);

            XElement xroot = xdoc.Root;

            if (xroot != null)
            {
                XElement source = xroot.Element("source") ?? new XElement("source", DEFAULT_SRC);
                XElement position = xroot.Element("position") ?? new XElement("position", DEFAULT_POS);

                return new SettingInfo(new Uri(source.Value), Convert.ToInt32(position.Value));
            }

            return new SettingInfo(new Uri(DEFAULT_SRC), Convert.ToInt32(DEFAULT_POS));
        }


        public SettingWindow()
        {
            InitializeComponent();
        }

        private void SettingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SettingInfo si = GetInformation();

            this.Source.Text = si.Source.AbsoluteUri;
            this.Position.SelectedIndex = si.Position;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            XDocument xdoc = new XDocument(
                new XElement("setting",
                    new XElement("source", this.Source.Text),
                    new XElement("position", this.Position.SelectedIndex)
                )
            );

            xdoc.Save(SETTING_XML);

            //((MainWindow)this.Owner).LoadUrl();
            this.Close();
        }
    }
}
