using System.Web;

namespace DMLTriggerGenerator.Utils
{
    public class HttpContextSessionWrapper
    {
        private T GetFromSession<T>(string key)
        {
            return (T)HttpContext.Current.Session[key];
        }

        private void SetInSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public string ConnectionString
        {
            get { return GetFromSession<string>("ConnectionString"); }
            set { SetInSession("ConnectionString", value); }
        }
    }
}
