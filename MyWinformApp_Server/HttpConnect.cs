using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;

namespace MyWinformApp_Server
{
    class HttpConnect
    {
        HttpListener httpListener;

        public void ServerInit()
        {
            if(httpListener == null)
            {
                httpListener = new HttpListener();

                // Prefixies는 API 호출을 위한 기본주소(첫번째 슬래시 "/" 까지의 주소)
                // "http://+:8686/" 에서 +라고 표기한 것은 해당 pc의 모든 ip로의 접근이라는 의미이며 8686은 접속 포트를 의미
                // 모든 IP : 127.0.0.1, localhost, 외부 IP, 내부 IP...
                // 즉, httpListener의 프리픽스를 현재 IP 뒤에 :8686이라고 붙은걸로 하겠다. 라는 의미.
                httpListener.Prefixes.Add(string.Format("http://+:8686/"));
                ServerStart();
            }
        }

        private void ServerStart()
        {
            if (!httpListener.IsListening)
            {
                httpListener.Start();
                // display server started

                Task.Factory.StartNew(() =>
                {
                    while (httpListener != null)
                    {
                        HttpListenerContext context = httpListener.GetContext();

                        string rawurl = context.Request.RawUrl;
                        string httpMethod = context.Request.HttpMethod;

                        string result = "";

                        result += string.Format("httpmethod = {0}\r\n", httpMethod);
                        result += string.Format("rawurl = {0}\r\n", rawurl);

                        // display result

                        context.Response.Close();
                    }
                });
            }
        }
    }
}
