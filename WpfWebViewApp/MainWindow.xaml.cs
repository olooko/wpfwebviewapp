using CefSharp;
using Microsoft.Web.WebView2.Core;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
using System.Windows.Threading;
using System.Xml.Linq;

namespace WpfWebViewApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _base64UpImage = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAH0SURBVHhe7ZBLUsMwEESzYM89+KwDR88NuAMngBWcIHSnuosAJrbHsi0586qUcmxp1K93SZIkSZIkSbI8x+Pxlkt/rwvJv2hdVwln8uZ6SqCohH+z/RIoKNH/2G4JFJNgH9srgUISG8p2SqCIhMbSfgkUkEiUdktgcAlMpb0SGFjBS9FOCQyqwKWpvwQGVNC5qLcEBlPAuamvBAZSsKWopwQGUaAI71oR1i+BARQkAsUfsR70HGG9EnixAkQ4yWsUZ7VVAi/UxRF+yBu8a6MEXqQLI3TKG3yruwReoIsiXJQ32FNnCRysCyIMkjfYW1cJHKjBEUbJG5y519kI5UrgIA2MEJI3OLtuCRygQREmyRvMWKcEHtSACEXkDWYtWwIP6GCEovIGM5cpgRt1IMIs8gazWcIbLwrQXwI3aGOEWeUN7pinBH7QhgiLyBvcVbYEvtCHCIvKG9xZpgQ+6EWEVeQN7p5eAn4Op7/jWVXeIMOUEg4csMf6OP0dThXyBlkiJXxi7T1gTAlVyRtkusMaWsK3vOELrL4SqpQ3yDakhL/yhh+w/iuhanmDjJdKoPyTtnaDDV0lNCFvkLWrhH55g43nJTQlb5D5vATKP+vTMHCAJbxiNSdvkJ0l0GGcvMHBGz02yxYckiRJkiRJkiRJCrPbfQHnv65TQMyqmwAAAABJRU5ErkJggg==";
        private const string _base64DownImage = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAHzSURBVHhe7ZBLUsMwEESzYJ978FkDR88NuAMngBWcIHSb7gJCEtsT2ZaceVVKObY06tebJEmSJEmSJEmS5ID9fn+jx2YJO+DgM9Yr1q1eNQeyP8jhUa+GgQOU/8Qib1jNlYDMlH+nAPjAGlYCNj5hWd40VQKy/pY3/SVgwzF500QJyHhM3pwugR+wTsmbqktAtnPy5n8JfIHVJ2+qLAGZhsibnxL4gDVU3rCEu25ABSDLGHnzXQJ+dt3f8VRRAjJE5M2OA7ZYL93f8SxaAu6+RJ7OWw9qrgTcWUbe8IU+RJi1BNxVVt7wgzZEmKUE3DGNvOEGbYwwaQmYPa284UYdiMCAxUvAzHnkDQ/oYISiJWDWvPKGBzUgQpESMGMZecMBGhThohJwdll5w0EaGCFUAs7UIW84UIMjUOReo3rB3rrkDQfrggiDSsCeOuUNL9BFEc6WgG91yxtepAsjHC0B79qQN7xQF0f4UwKe25I3vFgBInQlYLUpbxhAQSJQvF15wyAKNBf1yBsGUrCpqU/eMJgCTkW98oYBFbQ09csbBlXgUrQjbxhYwS+lPXnD4BKI0q68oYBExtK+vKGIhIayHnlDIYn1sT55QzEJnmK98oaCEj1k/fKGohI21yNvKCzx65M3KuE65ZMkSZIkSZJF2Wy+AMdurlOXoaKsAAAAAElFTkSuQmCC";
        private const string _base64NormalImage = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAENSURBVHhe7dtBSsNAGMXxjBdx4cYL6BUUyZFKS4/lokeoa4W0V5l+w7xVIdVkki58/x8M7wuEMLxFSRPSzZFz7mP95lunN4trDfWSN73r9EkelLYoQGmLApS2KEBpiwKUtihAacu+gBR/InrNU7zE2tZx1E9K6Vlzk9jjEPFUj0btYn3V8e9KAVnz0u5dwCz8BihtUYDSFgUobdkXUO4DFnt6e+Uc9wEfmpvEHj8jHusRAAAAAAAA0KI8EiuvndZwSim9aW4SezxErPJIjHeDSlsUoLRFAUpbFKC0Ve4D5nxp8RprX8dR974P2MQ61nFlsSE+mfkvKEBpiwKUtihAaYsClLYoQGnLvICuuwATWdR/tQ/JbwAAAABJRU5ErkJggg==";
        private const string _base64CloseImage = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAKkSURBVHhe7ZtRTuNAEAUjrgAcg3B/KdwBOAZwh1AP/JQgEscez4y7CSX1rtee6el62eVjI29Osd/vH6kddTvcSgsO99QT9TDcGoeFkv+gxDOVNgRml/yrROCdGg+BBcfyJmUIzHwsb86HwINT8iZVCMx6St4ohO2w9BtujMmbFCEw45i8OYTAxRR5EzoEZpsib+S81Sb9hJxDyBCYaY682WnjLfXy9cfpaH2YEJilRP7wQeqCShkCMyyTN7pBpQqBs+vIGz2gUoTAmXXljRZQoUPgrDbyRgupkhDuhhbN4Iy28oYNd1SoEOjdR96wMUwI9Owrb2iwegj0Wkfe0Gi1EOixrryhYfcQ2BtD3tC4WwjsiSVvOKB5CKyNKW84SCHMHVDrL4bAmtjyhgOrh8CzHPKGg6uFwL1c8oYBFofAdU55wyDFIVC55Q0DlYaQX94wWMmnOYe48oYBW4UQX94waO0Q8sgbBq4VQj55w+BLQ8grbxAoDaGL/M3w+z8t4BO83n8CDH69PwQZuJa8yRMCg9aWN/FDYMBW8iZuCAxWIi8h1RzihcBApfL6Ci7dt9I/YJBi+aGFeuQMgQEWyxvdo/KEwMHV5I2eUfFD4MDq8kZrqLghcFAzeaO1VEkIk798KYIDmssb9uj/GOOEQONu8oa9MUKgYXd5Q491Q6DRavKGXuuEQIPV5Q09+4bAxjDyht59QmBDOHnDGW1DYGFYecNZCmHujFo/HgILwssbzqwbAg/SyBvOrhMCN9LJG2ZYFgIXaeUNs5SHwC9/5ZWZkhCetPGB0ltUUwgpb5htzt/mN+r7HUJdUJdCCC1vmHFKCAd5w40tdS6EFPKGWcdC+C1veKAQ/vKrs+flDQuOQ0gpb5j9OITL8oaFepv0Cl6f32w+Aa/AR4WADNL/AAAAAElFTkSuQmCC";

        private string _defaultJavascriptString = string.Empty;

        private bool _isFullScreen;

        public MainWindow()
        {
            InitializeComponent();

            _defaultJavascriptString = ""
                + "function postMessageToAppCreatedByApp(key, data) {"
                + "  var webmessage = new Object();"
                + "  webmessage.key = key;"
                + "  webmessage.data = data;"
                + "  if (window.chrome.webview) window.chrome.webview.postMessage(JSON.stringify(webmessage));"
                + "  if (window.hasOwnProperty('CefSharp')) CefSharp.PostMessage(JSON.stringify(webmessage));"
                + "}"
                + "var touchCountCreatedByApp = 0;"
                + "var controlBoxCreatedByApp = document.createElement('div');"
                + "controlBoxCreatedByApp.style.cssText = 'cursor:pointer;z-index:1234567890;background-color:#5b5fc7;opacity:0;width:100px;height:100px;position:absolute;display:flex;';"
                + "controlBoxCreatedByApp.addEventListener('click', function(event) {"
                + "  event.stopPropagation();"
                + "  if (touchCountCreatedByApp === 0) { setTimeout('checkTouchCountCreatedByApp()', 3000); }"
                + "  touchCountCreatedByApp = touchCountCreatedByApp + 1;"
                + "  if (touchCountCreatedByApp >= 5) {"
                + "    controlBoxCreatedByApp.style.opacity = 1;"
                + "    controlBoxCreatedByApp.style.width = '100%';"
                + "    buttonDisplayCreatedByApp('inline');"
                + "  }"
                + "});"
                + "function checkTouchCountCreatedByApp() {"
                + "  if (touchCountCreatedByApp < 5) {"
                + "    touchCountCreatedByApp = 0;"
                + "    controlBoxCreatedByApp.style.opacity = 0;"
                + "    controlBoxCreatedByApp.style.width = '100px';"
                + "    buttonDisplayCreatedByApp('none');"
                + "  }"
                + "}"
                + "function buttonDisplayCreatedByApp(display) {"
                + "  hideButtonCreatedByApp.style.display = display;"
                + "  normalButtonCreatedByApp.style.display = display;"
                + "  closeButtonCreatedByApp.style.display = display;"
                + "}"
                + "var buttonStyleCreatedByApp = 'display:none;margin:4px 2px 4px 2px;width:92px;height:92px;border:2px solid black;background-color:#5b5fc7;color:white;border-color:white;cursor:pointer;border-radius:5px;';"
                + "var hideButtonCreatedByApp = document.createElement('button');"
                + "hideButtonCreatedByApp.style.cssText = buttonStyleCreatedByApp;"
                + "hideButtonCreatedByApp.addEventListener('click', function(event) {"
                + "  event.stopPropagation();"
                + "	 touchCountCreatedByApp = 0;"
                + "	 controlBoxCreatedByApp.style.opacity = 0;"
                + "	 controlBoxCreatedByApp.style.width = '100px';"
                + "	 buttonDisplayCreatedByApp('none');"
                + "});"
                + "var normalButtonCreatedByApp = document.createElement('button');"
                + "normalButtonCreatedByApp.style.cssText = buttonStyleCreatedByApp;"
                + string.Format("normalButtonCreatedByApp.innerHTML = '<img width=\"50px\" height=\"50px\" src=\"{0}\"/>';", _base64NormalImage)
                + "normalButtonCreatedByApp.addEventListener('click', function(event) {"
                + "  postMessageToAppCreatedByApp('wm-normal');"
                + "});"
                + "var closeButtonCreatedByApp = document.createElement('button');"
                + "closeButtonCreatedByApp.style.cssText = buttonStyleCreatedByApp;"
                + string.Format("closeButtonCreatedByApp.innerHTML = '<img width=\"50px\" height=\"50px\" src=\"{0}\" />';", _base64CloseImage)
                + "closeButtonCreatedByApp.addEventListener('click', function(event) {"
                + "  postMessageToAppCreatedByApp('wm-close');"
                + "});"           
                + "controlBoxCreatedByApp.appendChild(hideButtonCreatedByApp);"
                + "controlBoxCreatedByApp.appendChild(normalButtonCreatedByApp);"
                + "controlBoxCreatedByApp.appendChild(closeButtonCreatedByApp);"
                + "document.body.appendChild(controlBoxCreatedByApp);"
                + "if (window.chrome.webview) {"
                + "  window.chrome.webview.addEventListener('message', function(event) {"
                + "    MessageReceived(event.data);"
                + "  });"
                + "}"
                + "if (window.hasOwnProperty('CefSharp')) {"
                + "  document.addEventListener('cefmessage', function(event) {"
                + "    MessageReceived(event.detail);"
                + "  });"
                + "}"
                + "function MessageReceived(message) {"
                + "  controlBoxCreatedByApp.style.opacity = 1;"
                + "  controlBoxCreatedByApp.style.width = '100%';"
                + "  buttonDisplayCreatedByApp('inline');"
                + "}"
            ;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _isFullScreen = false;

            ApplyControlBarButtonState();

            ShowWebView(Setting.GetInformation());
        }

        private void ApplyControlBarButtonState()
        {
            if (_isFullScreen) 
            { 
                this.FullScreenExitImage.Visibility = Visibility.Visible;
                this.FullScreenImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.FullScreenExitImage.Visibility = Visibility.Collapsed;
                this.FullScreenImage.Visibility = Visibility.Visible;
            }

            if (this.WindowState == WindowState.Maximized)
            {
                this.MaximizeImage.Visibility = Visibility.Collapsed;
                this.NormalImage.Visibility = Visibility.Visible;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                this.MaximizeImage.Visibility = Visibility.Visible;
                this.NormalImage.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowWebView(Setting si, bool reload = false)
        {
            this.BusyIndicator.IsBusy = true;

            switch (si.WebViewType)
            {
                case WebViewType.Edge:
                    this.WV2.Visibility = Visibility.Visible;
                    this.CEF.Visibility = Visibility.Hidden;
                    this.WV2.Source = new Uri(si.Url);
                    this.WebViewVersion.Text = CoreWebView2Environment.GetAvailableBrowserVersionString();
                    this.EdgeImage.Visibility = Visibility.Visible;
                    this.ChromeImage.Visibility = Visibility.Collapsed;
                    //if (reload) this.WV2.Reload();
                    break;
                case WebViewType.Chrome:
                    this.WV2.Visibility = Visibility.Hidden;
                    this.CEF.Visibility = Visibility.Visible;
                    this.CEF.Address = si.Url;
                    this.WebViewVersion.Text = Cef.ChromiumVersion;
                    this.EdgeImage.Visibility = Visibility.Collapsed;
                    this.ChromeImage.Visibility = Visibility.Visible;
                    //if (reload) this.CEF.Reload();
                    break;
            }

            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    int releaseKey = (int)ndpKey.GetValue("Release");
                    string netVersion = string.Empty;

                    if (releaseKey >= 533320)
                        netVersion = "net481";
                    else if (releaseKey >= 528040)
                        netVersion = "net48";
                    else if (releaseKey >= 461808)
                        netVersion = "net472";
                    else if (releaseKey >= 461308)
                        netVersion = "net471";
                    else if (releaseKey >= 460798)
                        netVersion = "net47";
                    else if (releaseKey >= 394802)
                        netVersion = "net462";
                    else if (releaseKey >= 394254)
                        netVersion = "net461";
                    else if (releaseKey >= 393295)
                        netVersion = "net46";
                    else if (releaseKey >= 379893)
                        netVersion = "net452";
                    else if (releaseKey >= 378675)
                        netVersion = "net451";
                    else if (releaseKey >= 378389)
                        netVersion = "net45";
                    else
                        netVersion = "net45 under";

                    this.DotNetVersion.Text = string.Format("({0})", netVersion);
                }
                else
                {
                    this.DotNetVersion.Text = "(net45 under)";
                }
            }
        }

        private string GetJavascriptAllString()
        {
            string position = string.Empty;
            string image = string.Empty;

            Setting st = Setting.GetInformation();

            switch (st.Position)
            {
                case 0:
                    position = "justify-content:flex-end;top:0;left:0;";
                    image = _base64UpImage;
                    break;
                case 1:
                    position = "justify-content:flex-start;top:0;right:0;";
                    image = _base64UpImage;
                    break;
                case 2:
                    position = "justify-content:flex-end;bottom:0;left:0;";
                    image = _base64DownImage;
                    break;
                case 3:
                    position = "justify-content:flex-start;bottom:0;right:0;";
                    image = _base64DownImage;
                    break;
            }

            string jsString = _defaultJavascriptString;
            jsString += string.Format("controlBoxCreatedByApp.style.cssText += '{0}';", position);
            jsString += string.Format("hideButtonCreatedByApp.innerHTML = '<img width=\"50px\" height=\"50px\" src=\"{0}\"/>';", image);

            return jsString;
        }

        private void ControlBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
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

        private void SettingViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WV2 != null) this.WV2.Visibility = Visibility.Hidden;
            if (this.CEF != null) this.CEF.Visibility = Visibility.Hidden;

            this.SettingGrid.Visibility = Visibility.Visible;

            Setting st = Setting.GetInformation();

            switch (st.WebViewType)
            {
                case WebViewType.Edge:
                    this.Edge.IsChecked = true;
                    break;
                case WebViewType.Chrome:
                    this.Chrome.IsChecked = true;
                    break;
            }

            this.SettingUrl.Text = st.Url;
            this.SettingPosition.SelectedIndex = st.Position;
        }

        private void SettingSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WebViewType webviewType = WebViewType.Edge;

            if (this.Chrome.IsChecked == true)
                webviewType = WebViewType.Chrome;

            Setting.Save(webviewType, this.SettingUrl.Text, this.SettingPosition.SelectedIndex);

            this.SettingGrid.Visibility = Visibility.Hidden;

            ShowWebView(Setting.GetInformation(), true);
        }

        private void SettingCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.SettingGrid.Visibility = Visibility.Hidden;

            ShowWebView(Setting.GetInformation(), true);
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            _isFullScreen = !_isFullScreen;

            if (_isFullScreen)
            {
                this.WindowState = WindowState.Maximized;
                this.ControlBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                this.ControlBar.Visibility = Visibility.Visible;
            }

            ApplyControlBarButtonState();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;

            ApplyControlBarButtonState();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WV2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.WV2.ExecuteScriptAsync(GetJavascriptAllString());
                this.BusyIndicator.IsBusy = false;
            }));
        }

        private void CEF_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading == false)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    this.CEF.GetMainFrame().ExecuteJavaScriptAsync(GetJavascriptAllString());
                    this.BusyIndicator.IsBusy = false;
                }));               
            }
        }

        private void WV2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string s = e.TryGetWebMessageAsString();
            WebMessageReceived(s);
        }

        private void CEF_JavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            string s = e.Message as string;
            WebMessageReceived(s);
        }

        private void WebMessageReceived(string message)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                WebMessage webmessage = JsonConvert.DeserializeObject<WebMessage>(message);

                switch (webmessage.key)
                {
                    case "wm-normal":
                        _isFullScreen = false;
                        this.WindowState = WindowState.Normal;
                        this.ControlBar.Visibility = Visibility.Visible;
                        ApplyControlBarButtonState();
                        break;
                    case "wm-close":
                        this.Close();
                        break;
                    default:
                        break;
                }
            }));
        }

        // App to javascript
        //
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = string.Empty;

            Setting st = Setting.GetInformation();

            switch (st.WebViewType)
            {
                case WebViewType.Edge:
                    this.WV2.CoreWebView2.PostWebMessageAsString(message);
                    break;

                case WebViewType.Chrome:
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("var event = new CustomEvent('cefmessage', {{bubbles: true, detail:'{0}'}});", message);
                        sb.Append("document.dispatchEvent(event);");
                        this.CEF.GetMainFrame().ExecuteJavaScriptAsync(sb.ToString());
                    }               
                    break;
            }
                
        }

    }
}
