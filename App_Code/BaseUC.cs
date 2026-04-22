using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace Control
{
    public abstract class BaseUC : System.Web.UI.UserControl
    {
        private ResourceManager _rm;
        private CultureInfo _ci;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //// Set Language
            string lang = "en-US";
            if (Request.Cookies["MalaysiaTorayNaviLanguage"] != null)
            {
                lang = Request.Cookies["MalaysiaTorayNaviLanguage"].Value;
            }
            SetCulture(lang, lang);
        }

        public void VirtualSessionClear()
        {
            if (Session["KioskVirtualSession"] != null)
            {
                Session["KioskVirtualSession"] = null;
            }
        }

        public string VirtualSessionRead()
        {
            if (Session["KioskVirtualSession"] == null)
            {
                Session["KioskVirtualSession"] = DateTime.Now.Year + DateTime.Now.DayOfYear.ToString("000") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
            }

            return (string)Session["KioskVirtualSession"];
        }

        /// <summary>
        /// Set Language Culture
        /// </summary>
        protected void SetCulture(string name, string locale)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(name);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            _ci = Thread.CurrentThread.CurrentCulture;
        }

        public string LocalizeText(string Key)
        {
            if (_rm == null)
            {
                _rm = new ResourceManager("resources.Language", Assembly.Load("App_GlobalResources"));
            }

            return (Key != "") ? _rm.GetString(Key, Thread.CurrentThread.CurrentCulture).Trim() : "";
        }

        public enum LanguagePack
        {
            English = 0,
            Malay = 1
        }

        public LanguagePack Language
        {
            get
            {
                LanguagePack lp = LanguagePack.English;

                if (Thread.CurrentThread.CurrentCulture.ToString().Equals("ms-MY"))
                {
                    lp = LanguagePack.Malay;
                }

                return lp;
            }
        }

        public string ImageLanguagePrefix
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture.ToString().Equals("ms-MY") ? "_m" : "";
            }
        }
    }
}
