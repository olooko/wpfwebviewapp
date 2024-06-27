using CefSharp;
using Microsoft.Web.WebView2.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfWebViewApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();   
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            /*
            if (System.Environment.Is64BitOperatingSystem)
            {
                RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

                RegistryKey registryKey = baseKey.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}");

                if (registryKey != null)
                {
                    MessageBox.Show("x64 LocalMachine 설치");
                }

                baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);

                registryKey = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}");

                if (registryKey != null)
                {
                    MessageBox.Show("x64 CurrentUser 설치");
                }
            }
            else
            {
                RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

                RegistryKey registryKey = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}");

                if (registryKey != null)
                {
                    MessageBox.Show("x86 LocalMachine 설치");
                }

                baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);

                registryKey = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}");

                if (registryKey != null)
                {
                    MessageBox.Show("x86 CurrentUser 설치");
                }
            }












            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    MessageBox.Show($".NET Framework Version: {CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}");
                }
                else
                {
                    MessageBox.Show(".NET Framework Version 4.5 or later is not detected.");
                }
            }
            */
        }

        // Checking the version using >= enables forward compatibility.
        string CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 533320)
                return "4.8.1 or later";
            if (releaseKey >= 528040)
                return "4.8";
            if (releaseKey >= 461808)
                return "4.7.2";
            if (releaseKey >= 461308)
                return "4.7.1";
            if (releaseKey >= 460798)
                return "4.7";
            if (releaseKey >= 394802)
                return "4.6.2";
            if (releaseKey >= 394254)
                return "4.6.1";
            if (releaseKey >= 393295)
                return "4.6";
            if (releaseKey >= 379893)
                return "4.5.2";
            if (releaseKey >= 378675)
                return "4.5.1";
            if (releaseKey >= 378389)
                return "4.5";
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            ShowWebView((WebViewName)Enum.Parse(typeof(WebViewName), rb.Name));
        }

        private void TopGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ShowWebView(WebViewName name)
        {
            switch (name)
            {
                case WebViewName.Edge:
                    this.MainDesc.Text = "Edge";
                    if (this.WV2 != null) this.WV2.Visibility = Visibility.Visible;
                    if (this.CEF != null) this.CEF.Visibility = Visibility.Collapsed;
                    break;
                case WebViewName.Chrome:
                    this.MainDesc.Text = "Chrome";
                    if (this.WV2 != null) this.WV2.Visibility = Visibility.Collapsed;
                    if (this.CEF != null) this.CEF.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            if (this.WV2 != null) this.WV2.Dispose();
            if (this.CEF != null) this.CEF.Dispose();

            Process[] ps = Process.GetProcesses();

            foreach (var p in ps)
            {
                if (p.ProcessName.Equals("msedgewebview2"))
                {
                    try
                    {
                        p.Kill();
                    }
                    catch { }
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WV2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string s = e.TryGetWebMessageAsString();
            MessageBox.Show(s);
        }

        private void CEF_JavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            string s = e.Message as string;
            MessageBox.Show(s);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)this.Edge.IsChecked)
            {
                this.WV2.CoreWebView2.PostWebMessageAsString("WV2 Alert!!");
            }

            if ((bool)this.Chrome.IsChecked)
            {
               //if (this.CEF != null) this.CEF.ShowDevTools();

                string script = "var event = new CustomEvent('cefmessage', {bubbles: true, detail:'CEF Alert'}); document.dispatchEvent(event);";
                 
                this.CEF.GetMainFrame().ExecuteJavaScriptAsync(script);
            }
        }
    }
}
