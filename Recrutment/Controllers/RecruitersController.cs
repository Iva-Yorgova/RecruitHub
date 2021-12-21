using MyWebServer.Controllers;
using Recrutment.Data;
namespace Recrutment.Controllers
{
    public class RecruitersController : Controller
    {
        private readonly RecrutmentDbContext data;

        public RecruitersController(RecrutmentDbContext data)
            => this.data = data;


    }
}
