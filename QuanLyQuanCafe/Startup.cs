using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuanLyQuanCafe.Startup))]
namespace QuanLyQuanCafe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
