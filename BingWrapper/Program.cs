using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BingWrapper
{
    class Program
    {
        public static string GetHttpData(string uri)
        {
            Uri url = new Uri(uri);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return result;
        }

        public static void DownloadImage(string url)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, Path.GetFileName(url));
        }

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public static void SetWrapper(string path)
        {
            SystemParametersInfo(20, 0, path, 0x01 | 0x02);
        }

        static void Main(string[] args)
        {
            string str = GetHttpData("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1");
            int index = str.IndexOf("http");
            str = str.Substring(index);
            index = str.IndexOf("\"");
            str = str.Substring(0, index);
            DownloadImage(str);
            SetWrapper(Directory.GetCurrentDirectory() + "\\" + Path.GetFileName(str));
        }
    }
}
