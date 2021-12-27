using MyWebServer;
using MyWebServer.Controllers;
using MyWebServer.Results.Views;
using Recrutment.Data;
using Recrutment.Services;
using System.Threading.Tasks;

namespace Recrutment
{
    public class Startup
    {
            public static async Task Main()
            => await HttpServer
                .WithRoutes(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                .WithServices(services => services
                    .Add<RecrutmentDbContext>()
                    .Add<IValidator, Validator>()
                    .Add<IPasswordHasher, PasswordHasher>()
                    .Add<IViewEngine, CompilationViewEngine>())
                .WithConfiguration<RecrutmentDbContext>(context => context.Database.EnsureCreated())
                .Start();
        }

    }

