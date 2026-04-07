using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RutinApp.Shared.Data;
using RutinApp.ViewModels;
using RutinApp.Views;

namespace RutinApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlite($"Filename={dbPath}"));

            // ViewModels
            builder.Services.AddSingleton<EjercicioViewModel>();
            builder.Services.AddSingleton<RutinaViewModel>();
            // Views
            builder.Services.AddTransient<EjerciciosPage>();
            builder.Services.AddTransient<AddEjercicioPage>();
            builder.Services.AddTransient<MisRutinasPage>();
            builder.Services.AddTransient<AddRutinaPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dataBase = scope.ServiceProvider.GetRequiredService<DataContext>();
                dataBase.Database.Migrate();
            }

            return app;
        }
    }
}
