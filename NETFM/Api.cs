using System.IO;
using System.Net;
using System.Text;

namespace NETFM
{
    public class Api
    {
        public static string CountSize(double Size)
        {
            string m_strSize = "";
            double FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }
        
        public static string GetHTML(string url,  string Host = "")
        {
            try
            {
                HttpWebRequest t = (HttpWebRequest)WebRequest.Create(url);
                if (Host.Trim() != "")
                {
                    t.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    t.Host = Host;
                }
                t.Method = "GET";
                t.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0";
                HttpWebResponse p = (HttpWebResponse)t.GetResponse();
                StreamReader r = new StreamReader(p.GetResponseStream(), Encoding.UTF8);

                string c = r.ReadToEnd();
                r.Close();
                return c;
                //return ""; 
            }
            catch { return ""; }
        }
    }
}
