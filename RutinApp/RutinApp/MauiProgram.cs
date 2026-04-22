using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RutinApp.Shared.Data;
using RutinApp.Shared.Models;
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

            builder.Services.AddDbContextFactory<DataContext>(options =>
                options.UseSqlite($"Filename={dbPath}"));

            builder.Services.AddSingleton<EjercicioViewModel>();
            builder.Services.AddSingleton<RutinaViewModel>();

            builder.Services.AddTransient<EjerciciosPage>();
            builder.Services.AddTransient<AddEjercicioPage>();
            builder.Services.AddTransient<MisRutinasPage>();
            builder.Services.AddTransient<AddRutinaPage>();

            builder.Services.AddSingleton<SesionViewModel>();
            builder.Services.AddTransient<SesionPage>();

            builder.Services.AddTransient<HistorialViewModel>();
            builder.Services.AddTransient<HistorialPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DataContext>>();
                using var db = factory.CreateDbContext();
                db.Database.Migrate();

                // Crear usuario por defecto si no existe
                if (!db.Usuarios.Any())
                {
                    db.Usuarios.Add(new UsuarioModel
                    {
                        Nombre = "Usuario",
                        Email = "usuario@rutinapp.com",
                        Password = "default"
                    });
                    db.SaveChanges();
                }
            }

            return app;
        }
    }
}