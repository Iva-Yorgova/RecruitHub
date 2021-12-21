
using MyWebServer.Controllers;
using MyWebServer.Http;

namespace Recrutment.Controllers
{
    public class HomeController : Controller
    {
        public HttpResponse Index()
        {
     

            return this.View();
        }
    }
}
