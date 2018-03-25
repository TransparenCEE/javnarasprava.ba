using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Infrastructure
{
    [Serializable]
    public sealed class SessionManager
    {
        private const string SESSION_MANAGER = "SESSION_MANAGER";

        public static SessionManager Current
        {
            get
            {
                HttpContext context = HttpContext.Current;
                SessionManager manager = context.Session[ SESSION_MANAGER ] as SessionManager;

                if ( manager == null )
                {
                    manager = new SessionManager();
                    context.Session[ SESSION_MANAGER ] = manager;
                }

                return manager;
            }
        }
        public int CurrentParliamentId { get; set; }
    }
}