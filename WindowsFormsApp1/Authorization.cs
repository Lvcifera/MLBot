using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public class Authorization
    {
        private CookieContainer cookie = new CookieContainer();

        private string Md5(string input)
        {
            var x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bs = Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            var s = new StringBuilder();
            foreach(var b in bs) { s.Append(b.ToString("x2").ToLower()); }
            return s.ToString();
        }

        public string getMain()
        {
            string result = "";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://mlgame.ru");
            request.KeepAlive = true;
            request.Headers.Add("Accept-Language", "ru,ru-ru;q=0.8,en;q=0.5,en-us;q=0.3");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("DNT", "1");
            request.CookieContainer = cookie;
            //request.ContentType = "application/x-amf";
            request.ServicePoint.Expect100Continue = false;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using(Stream stream = response.GetResponseStream())
            {
                using(StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            response.Close();
            return result;
        }

        public string getSalt()
        {
            string result = "";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://mlgame.ru/salt");
            request.KeepAlive = true;
            request.Headers.Add("Accept-Language", "ru,ru-ru;q=0.8,en;q=0.5,en-us;q=0.3");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("DNT", "1");
            request.CookieContainer = cookie;
            //request.ContentType = "application/x-amf";
            request.ServicePoint.Expect100Continue = false;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using(Stream stream = response.GetResponseStream())
            {
                using(StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    result = json.Substring(9, json.Length - 9 - 2);
                }
            }
            response.Close();
            return result;
        }

        public string getSaltPasswordHash(string salt, string password)
        {
            return Md5(Md5(password) + salt);
        }

        public string Authorize(string login, string resultPassword, string version)
        {
            string result = "";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://mlgame.ru/auth?username={0}&password={1}&version={2}", login, resultPassword, version));

            request.KeepAlive = true;
            request.Headers.Add("Accept-Language", "ru,ru-ru;q=0.8,en;q=0.5,en-us;q=0.3");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.ServicePoint.Expect100Continue = false;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)";
            request.CookieContainer = cookie;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using(Stream stream = response.GetResponseStream())
            {
                using(StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            response.Close();
            return result;
        }

        public string FindVersion(string datasession)
        {
            var htmBody = datasession;
            if(htmBody.Contains("version:"))
            {
                var t = Regex.Match(htmBody, @"version:\s""(\w+)""");
                var r = t.Value;
                var t2 = Regex.Replace(r, @"version:\s""(\w+)""", "$1");
                return t2.ToString();
            }

            return string.Empty;
        }
    }
}