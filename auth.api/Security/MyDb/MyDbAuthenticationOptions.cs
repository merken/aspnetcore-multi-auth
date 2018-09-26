using Microsoft.AspNetCore.Authentication;

namespace auth.api.Security.MyDb
{
    public class MyDbAuthenticationOptions : AuthenticationSchemeOptions
    {
        public MyDbAuthenticationOptions()
        {
        }

        public new MyDbAuthenticationEvents Events
        {
            get { return (MyDbAuthenticationEvents)base.Events; }

            set { base.Events = value; }
        }
    }
}