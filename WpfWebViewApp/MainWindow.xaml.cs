using CefSharp;
using Microsoft.Web.WebView2.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Xml.Linq;

namespace WpfWebViewApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _base64UpImg = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAH0SURBVHhe7ZBLUsMwEESzYM89+KwDR88NuAMngBWcIHSnuosAJrbHsi0586qUcmxp1K93SZIkSZIkSbI8x+Pxlkt/rwvJv2hdVwln8uZ6SqCohH+z/RIoKNH/2G4JFJNgH9srgUISG8p2SqCIhMbSfgkUkEiUdktgcAlMpb0SGFjBS9FOCQyqwKWpvwQGVNC5qLcEBlPAuamvBAZSsKWopwQGUaAI71oR1i+BARQkAsUfsR70HGG9EnixAkQ4yWsUZ7VVAi/UxRF+yBu8a6MEXqQLI3TKG3yruwReoIsiXJQ32FNnCRysCyIMkjfYW1cJHKjBEUbJG5y519kI5UrgIA2MEJI3OLtuCRygQREmyRvMWKcEHtSACEXkDWYtWwIP6GCEovIGM5cpgRt1IMIs8gazWcIbLwrQXwI3aGOEWeUN7pinBH7QhgiLyBvcVbYEvtCHCIvKG9xZpgQ+6EWEVeQN7p5eAn4Op7/jWVXeIMOUEg4csMf6OP0dThXyBlkiJXxi7T1gTAlVyRtkusMaWsK3vOELrL4SqpQ3yDakhL/yhh+w/iuhanmDjJdKoPyTtnaDDV0lNCFvkLWrhH55g43nJTQlb5D5vATKP+vTMHCAJbxiNSdvkJ0l0GGcvMHBGz02yxYckiRJkiRJkiRJCrPbfQHnv65TQMyqmwAAAABJRU5ErkJggg==";
        private string _base64DownImg = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAHzSURBVHhe7ZBLUsMwEESzYJ978FkDR88NuAMngBWcIHSb7gJCEtsT2ZaceVVKObY06tebJEmSJEmSJEmS5ID9fn+jx2YJO+DgM9Yr1q1eNQeyP8jhUa+GgQOU/8Qib1jNlYDMlH+nAPjAGlYCNj5hWd40VQKy/pY3/SVgwzF500QJyHhM3pwugR+wTsmbqktAtnPy5n8JfIHVJ2+qLAGZhsibnxL4gDVU3rCEu25ABSDLGHnzXQJ+dt3f8VRRAjJE5M2OA7ZYL93f8SxaAu6+RJ7OWw9qrgTcWUbe8IU+RJi1BNxVVt7wgzZEmKUE3DGNvOEGbYwwaQmYPa284UYdiMCAxUvAzHnkDQ/oYISiJWDWvPKGBzUgQpESMGMZecMBGhThohJwdll5w0EaGCFUAs7UIW84UIMjUOReo3rB3rrkDQfrggiDSsCeOuUNL9BFEc6WgG91yxtepAsjHC0B79qQN7xQF0f4UwKe25I3vFgBInQlYLUpbxhAQSJQvF15wyAKNBf1yBsGUrCpqU/eMJgCTkW98oYBFbQ09csbBlXgUrQjbxhYwS+lPXnD4BKI0q68oYBExtK+vKGIhIayHnlDIYn1sT55QzEJnmK98oaCEj1k/fKGohI21yNvKCzx65M3KuE65ZMkSZIkSZJF2Wy+AMdurlOXoaKsAAAAAElFTkSuQmCC";

        private string _defaultJavascriptString = ""
            + "var eventCreatedByVrauz = new Event('messageFromWindow');"
            + "window.chrome.webview.addEventListener('message', function(event) {"
            + "  eventCreatedByVrauz.data = event.data;"
            + "  document.dispatchEvent(eventCreatedByVrauz);"
            + "});"
            + "function messageToWindow(key, data) {"
            + "  postMessageToWindowChromeWebViewCreatedByVrauz(key, data);"
            + "}"
            + "function postMessageToWindowChromeWebViewCreatedByVrauz(key, data) {"
            + "  var message = new Object();"
            + "  message.key = key;"
            + "  message.data = data;"
            + "  if (window.chrome.webview !== undefined)"
            + "    window.chrome.webview.postMessage(message);"
            + "}"
            + "var touchCountCreatedByVrauz = 0;"
            + "var controlBoxCreatedByVrauz = document.createElement('div');"
            + "controlBoxCreatedByVrauz.style.cssText = 'cursor:pointer;z-index:1234567890;background-color:#FB8633;opacity:0;width:100px;height:100px;position:absolute;';"
            + "controlBoxCreatedByVrauz.addEventListener('click', function(event) {"
            + "  event.stopPropagation();"
            + "  if (touchCountCreatedByVrauz === 0) { setTimeout('checkTouchCountCreatedByVrauz()', 3000); }"
            + "  touchCountCreatedByVrauz = touchCountCreatedByVrauz + 1;"
            + "  if (touchCountCreatedByVrauz >= 5) {"
            + "    controlBoxCreatedByVrauz.style.opacity = 1;"
            + "    controlBoxCreatedByVrauz.style.width = '100%';"
            + "    buttonDisplayCreatedByVrauz('inline');"
            + "  }"
            + "});"
            + "function checkTouchCountCreatedByVrauz() {"
            + "  if (touchCountCreatedByVrauz < 5) {"
            + "    touchCountCreatedByVrauz = 0;"
            + "    controlBoxCreatedByVrauz.style.opacity = 0;"
            + "    controlBoxCreatedByVrauz.style.width = '100px';"
            + "    buttonDisplayCreatedByVrauz('none');"
            + "  }"
            + "}"
            + "function buttonDisplayCreatedByVrauz(display) {"
            + "  hideButtonCreatedByVrauz.style.display = display;"
            + "  backButtonCreatedByVrauz.style.display = display;"
            + "  forwardButtonCreatedByVrauz.style.display = display;"
            + "  settingButtonCreatedByVrauz.style.display = display;"
            + "  normalButtonCreatedByVrauz.style.display = display;"
            + "  fullscreenButtonCreatedByVrauz.style.display = display;"
            + "  closeButtonCreatedByVrauz.style.display = display;"
            + "}"
            + "var buttonStyleCreatedByVrauz = 'display:none;margin:4px 2px 4px 2px;width:92px;height:92px;border:2px solid black;background-color:#FB8633;color:white;border-color:white;cursor:pointer;border-radius:5px;';"
            + "var hideButtonCreatedByVrauz = document.createElement('button');"
            + "hideButtonCreatedByVrauz.style.cssText = buttonStyleCreatedByVrauz;"
            + "hideButtonCreatedByVrauz.addEventListener('click', function(event) {"
            + "  event.stopPropagation();"
            + "	 touchCountCreatedByVrauz = 0;"
            + "	 controlBoxCreatedByVrauz.style.opacity = 0;"
            + "	 controlBoxCreatedByVrauz.style.width = '100px';"
            + "	 buttonDisplayCreatedByVrauz('none');"
            + "});"
            + "var backButtonCreatedByVrauz = document.createElement('button');"
            + "backButtonCreatedByVrauz.style.cssText = buttonStyleCreatedByVrauz;"
            + "backButtonCreatedByVrauz.innerHTML = '<img width=\"50px\" height=\"50px\" src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAExSURBVHhe7ZlRisJAEAUTcW+43mbX9T6rZ4zVmZdlBZHOl8z0K2iGSPqjChJEp3eyLMslRpe1kPxGrQgh3LwfqBEhRJvvU8aOEILN8yVjRgix5pfipLUxQGiP/C9z1Gr/IGP5JJbXav8gY/kkltdq/yBj+SSW12r/IGP5JJbXav8gY/kkltdq/yBj+SSW12r/IGP5JJbXapfMOleQid/kf9pVDf4CVJQP1gBV5YO5snxw0FkWPwI6a78EN6q/D1YiApNlrG+BG0g5AlKOgJQjIOUISDkCUnsiXBlHYByBcQTGERhHYMpHuDGOwDgC4wiMIzAfWh0HpBwBKUdAyhGQcgSkHAGpPRE+tTYWiGUifOn2MUHwVYRv3TY2iD6LUEN+A+H/Ec76uBaIR4Q3/hc5TXfF+/ahyVEEVQAAAABJRU5ErkJggg==\"/>';"
            + "backButtonCreatedByVrauz.addEventListener('click', function(event) {"
            + "  window.history.back();"
            + "});"
            + "var forwardButtonCreatedByVrauz = document.createElement('button');"
            + "forwardButtonCreatedByVrauz.style.cssText = buttonStyleCreatedByVrauz;"
            + "forwardButtonCreatedByVrauz.innerHTML = '<img width=\"50px\" height=\"50px\" src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAEtSURBVHhe7dZbisJQEEXRROgeo45G0z0fHyOMu+rmBoMi9SdWnQUXH3/7EBOHT5rn+WRn+VgL4ROnqzUCwX+te6PGCIS+iu9yj0Dgf+t8K+cIhB1aX0i+EYj64Zw9L0YjQCNAI0AjQCNAI0AjQCNAI0AjQCNAI0AjQCNAI0AjQCNAI0AjQCNAI+BphNG+Xd5XMY3juA6xW14rOT5eCRUHMOsIFX8Cj6aqV8Cq8gB+M6w6wPokqDjA5jH41bh/2x+hi93Ig3KEG2IU71kxik+BGMV7Vky6+KtnxSg+BWIU71kxik+BmPLxN8+KUXwKxCjes2JSxf9yFB+k+BSIUbxnxSg+DYL2rSskV3xnYa3vrZzxnQW2zpdyx3cW2no3asR3Fty6Xa34zsI/Gz8Md0Ka8CYYPNOuAAAAAElFTkSuQmCC\"/>';"
            + "forwardButtonCreatedByVrauz.addEventListener('click', function(event) {"
            + "  window.history.forward();"
            + "});"
            + "var settingButtonCreatedByVrauz = document.createElement('button');"
            + "settingButtonCreatedByVrauz.style.cssText = buttonStyleCreatedByVrauz + 'float:right;';"
            + "settingButtonCreatedByVrauz.innerHTML = '<img width=\"50px\" height=\"50px\" src=\"data:image/png;base64,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\"/>';"
            + "settingButtonCreatedByVrauz.addEventListener('click', function(event) {"
            + "  postMessageToWindowChromeWebViewCreatedByVrauz('ws-setting');"
            + "});"
            + "var normalButtonCreatedByVrauz = document.createElement('button');"
            + "normalButtonCreatedByVrauz.style.cssText = buttonStyleCreatedByVrauz + 'float:right;';"
            + "normalButtonCreatedByVrauz.innerHTML = '<img width=\"50px\" height=\"50px\" src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAENSURBVHhe7dtBSsNAGMXxjBdx4cYL6BUUyZFKS4/lokeoa4W0V5l+w7xVIdVkki58/x8M7wuEMLxFSRPSzZFz7mP95lunN4trDfWSN73r9EkelLYoQGmLApS2KEBpiwKUtihAacu+gBR/InrNU7zE2tZx1E9K6Vlzk9jjEPFUj0btYn3V8e9KAVnz0u5dwCz8BihtUYDSFgUobdkXUO4DFnt6e+Uc9wEfmpvEHj8jHusRAAAAAAAA0KI8EiuvndZwSim9aW4SezxErPJIjHeDSlsUoLRFAUpbFKC0Ve4D5nxp8RprX8dR974P2MQ61nFlsSE+mfkvKEBpiwKUtihAaYsClLYoQGnLvICuuwATWdR/tQ/JbwAAAABJRU5ErkJggg==\"/>';"
            + "normalButtonCreatedByVrauz.addEventListener('click', function(event) {"
            + "  postMessageToWindowChromeWebViewCreatedByVrauz('ws-normal');"
            + "});"
            + "var fullscreenButtonCreatedByVrauz = document.createElement('button');"
            + "fullscreenButtonCreatedByVrauz.style.cssText = buttonStyleCreatedByVrauz + 'float:right;';"
            + "fullscreenButtonCreatedByVrauz.innerHTML = '<img width=\"50px\" height=\"50px\" src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAERSURBVHhe7dtBagIxGMXxiYseQ6h2MeeoYG/cYk/SQkHoLXSjX8hzZ9OMMxam7/+D4SUwQnwLmQwxnUJ3H58ppV7jUWKJXxHrMpvWQmmLApS2KEBpiwKUtuwLaH0Q2sT1XYbNjvEgNPQzV8USlxEPZdZsFdeuDCtyAQ2edPtsxJr7svQ6fgOUtihAaYsClLYoQGmLApS27AvIm6EPjWu2U21s/kp8r8eItzIDAAAAAAAALvIrsXwI8TfPM3wlls8HvJdZRdzYgvMB/xUFKG1RgNIWBShtUYDSFgUobbUeln6Ja+hm6BAbqL3Go8QS88Zm6GHpfD7gtQx/1lrALfjb3BxQgNIWBShtUYDSlnkBXXcGGoTV7VEJb6kAAAAASUVORK5CYII=\"/>';"
            + "fullscreenButtonCreatedByVrauz.addEventListener('click', function(event) {"
            + "  postMessageToWindowChromeWebViewCreatedByVrauz('ws-fullscreen');"
            + "});"
            + "var closeButtonCreatedByVrauz = document.createElement('button');"
            + "closeButtonCreatedByVrauz.style.cssText = buttonStyleCreatedByVrauz + 'float:right;';"
            + "closeButtonCreatedByVrauz.innerHTML = '<img width=\"50px\" height=\"50px\" src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAKkSURBVHhe7ZtRTuNAEAUjrgAcg3B/KdwBOAZwh1AP/JQgEscez4y7CSX1rtee6el62eVjI29Osd/vH6kddTvcSgsO99QT9TDcGoeFkv+gxDOVNgRml/yrROCdGg+BBcfyJmUIzHwsb86HwINT8iZVCMx6St4ohO2w9BtujMmbFCEw45i8OYTAxRR5EzoEZpsib+S81Sb9hJxDyBCYaY682WnjLfXy9cfpaH2YEJilRP7wQeqCShkCMyyTN7pBpQqBs+vIGz2gUoTAmXXljRZQoUPgrDbyRgupkhDuhhbN4Iy28oYNd1SoEOjdR96wMUwI9Owrb2iwegj0Wkfe0Gi1EOixrryhYfcQ2BtD3tC4WwjsiSVvOKB5CKyNKW84SCHMHVDrL4bAmtjyhgOrh8CzHPKGg6uFwL1c8oYBFofAdU55wyDFIVC55Q0DlYaQX94wWMmnOYe48oYBW4UQX94waO0Q8sgbBq4VQj55w+BLQ8grbxAoDaGL/M3w+z8t4BO83n8CDH69PwQZuJa8yRMCg9aWN/FDYMBW8iZuCAxWIi8h1RzihcBApfL6Ci7dt9I/YJBi+aGFeuQMgQEWyxvdo/KEwMHV5I2eUfFD4MDq8kZrqLghcFAzeaO1VEkIk798KYIDmssb9uj/GOOEQONu8oa9MUKgYXd5Q491Q6DRavKGXuuEQIPV5Q09+4bAxjDyht59QmBDOHnDGW1DYGFYecNZCmHujFo/HgILwssbzqwbAg/SyBvOrhMCN9LJG2ZYFgIXaeUNs5SHwC9/5ZWZkhCetPGB0ltUUwgpb5htzt/mN+r7HUJdUJdCCC1vmHFKCAd5w40tdS6EFPKGWcdC+C1veKAQ/vKrs+flDQuOQ0gpb5j9OITL8oaFepv0Cl6f32w+Aa/AR4WADNL/AAAAAElFTkSuQmCC\" />';"
            + "closeButtonCreatedByVrauz.addEventListener('click', function(event) {"
            + "  postMessageToWindowChromeWebViewCreatedByVrauz('ws-close');"
            + "});"
            + "controlBoxCreatedByVrauz.appendChild(hideButtonCreatedByVrauz);"
            + "controlBoxCreatedByVrauz.appendChild(backButtonCreatedByVrauz);"
            + "controlBoxCreatedByVrauz.appendChild(forwardButtonCreatedByVrauz);"
            + "controlBoxCreatedByVrauz.appendChild(closeButtonCreatedByVrauz);"
            + "controlBoxCreatedByVrauz.appendChild(fullscreenButtonCreatedByVrauz);"
            + "controlBoxCreatedByVrauz.appendChild(normalButtonCreatedByVrauz);"
            + "controlBoxCreatedByVrauz.appendChild(settingButtonCreatedByVrauz);"
            + "document.body.appendChild(controlBoxCreatedByVrauz);"
        ;

        public MainWindow()
        {
            InitializeComponent();   
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ApplyMaximizeButtonState();

            ShowWebView(Setting.GetInformation());

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

        private void ApplyMaximizeButtonState()
        {
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
        }

        private void TopBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ShowWebView(Setting si)
        {
            switch (si.WebViewType)
            {
                case WebViewType.Edge:
                    if (this.WV2 != null) this.WV2.Visibility = Visibility.Visible;
                    if (this.CEF != null) this.CEF.Visibility = Visibility.Hidden;
                    break;
                case WebViewType.Chrome:
                    if (this.WV2 != null) this.WV2.Visibility = Visibility.Hidden;
                    if (this.CEF != null) this.CEF.Visibility = Visibility.Visible;
                    break;
            }

            switch (si.WebViewType)
            {
                case WebViewType.Edge:
                    if (this.WV2 != null) this.WV2.Source = new Uri(si.Url);
                    break;
                case WebViewType.Chrome:
                    if (this.CEF != null) this.CEF.Address = si.Url;
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

            ApplyMaximizeButtonState();
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

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if ((bool)this.Edge.IsChecked)
        //    {
        //        this.WV2.CoreWebView2.PostWebMessageAsString("WV2 Alert!!");
        //    }

        //    if ((bool)this.Chrome.IsChecked)
        //    {
        //       //if (this.CEF != null) this.CEF.ShowDevTools();

        //        string script = "var event = new CustomEvent('cefmessage', {bubbles: true, detail:'CEF Alert'}); document.dispatchEvent(event);";

        //        this.CEF.GetMainFrame().ExecuteJavaScriptAsync(script);
        //    }
        //}

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

            ShowWebView(Setting.GetInformation());
        }
    }
}
