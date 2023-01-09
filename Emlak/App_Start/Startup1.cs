using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Emlak.App_Start.Startup1))]

namespace Emlak.App_Start
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions()
            {
              AuthenticationType="ApplicationCookie",
              LoginPath=new PathString("/Account/Login")
            });
        }
    }
}
