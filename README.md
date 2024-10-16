## WebView App using WebView2 and CefSharp on net4.8

### JSON String Definition 
Javascript
```
var webmessage = new Object();
webmessage.key = key;
webmessage.data = data;
```
WebView
```
public class WebMessage
{
    public string key { get; set; }
    public string data { get; set; }
}
```
### From Javascript To WebView With JSON String 
From Javascript
```
var webmessage = new Object();
webmessage.key = '';
webmessage.data = '';

var message = JSON.stringify(webmessage);

if (window.chrome.webview) window.chrome.webview.postMessage(message);
if (window.hasOwnProperty('CefSharp')) CefSharp.PostMessage(message);
```
To WebView
```
WebMessage webmessage = JsonConvert.DeserializeObject<WebMessage>(message);
```
### From WebView To Javascript With JSON String 
From WebView
```
WebMessage webmessage = new WebMessage();
webmessage.key = string.Empty;
webmessage.data = string.Empty;

string message = JsonConvert.SerializeObject(webmessage);

#if EDGE
    WebView2.CoreWebView2.PostWebMessageAsString(message);
#endif

#if CHROME
    StringBuilder sb = new StringBuilder();
    sb.AppendFormat("var event = new CustomEvent('cefmessage', {{bubbles: true, detail:'{0}'}});", message);
    sb.Append("document.dispatchEvent(event);");
    CefSharp.GetMainFrame().ExecuteJavaScriptAsync(sb.ToString());
#endif
```
To Javascript
```
if (window.chrome.webview) {
    window.chrome.webview.addEventListener('message', function(event) {
        MessageReceived(event.data);
    });
}
if (window.hasOwnProperty('CefSharp')) {
    document.addEventListener('cefmessage', function(event) {
        MessageReceived(event.detail);
    });
}  
```



