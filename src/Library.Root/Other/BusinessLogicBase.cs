using System.Threading;

namespace Library.Root.Other
{
    /// <summary>
    /// Handler the Common Business Logic Function.
    ///
    /// NOTE (.NET 8 migration): the original Web Forms version read
    /// <c>MaxRowPerPage</c> from <c>ConfigurationManager.AppSettings</c>.
    /// In ASP.NET Core, settings are loaded from <c>appsettings.json</c> via
    /// <c>IConfiguration</c>. The host application is expected to set
    /// <see cref="MaxQuantityPerPage"/> at startup (see
    /// <c>Program.cs</c> in <c>FLM_LobbyDisplay.Web</c>).
    /// </summary>
    public abstract class BusinessLogicBase
    {
        public enum LanguagePack
        {
            English = 0,
            Malay = 1
        }

        public static LanguagePack Language
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

        /// <summary>
        /// Max Quantity per Page. Defaults to 10 to match the legacy
        /// <c>web.config</c> default; the host app should overwrite this at
        /// startup from configuration.
        /// </summary>
        public static int MaxQuantityPerPage { get; set; } = 10;

        /// <summary>
        /// Generate and Caculate the Number
        /// </summary>
        public static int FromRowNo(int PageNo)
        {
            if (PageNo == 1)
            {
                return 1;
            }
            else
            {
                return ((PageNo - 1) * MaxQuantityPerPage) + 1;
            }
        }

        /// <summary>
        /// Generate and Caculate the Number
        /// </summary>
        public static int ToRowNo(int PageNo)
        {
            if (PageNo == 1)
            {
                return MaxQuantityPerPage;
            }
            else
            {
                return ((PageNo - 1) * MaxQuantityPerPage) + MaxQuantityPerPage;
            }
        }
    }
}
