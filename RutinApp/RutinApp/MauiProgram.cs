using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RutinApp.Shared.Data;
using RutinApp.Shared.Models;
using RutinApp.ViewModels;
using RutinApp.Views;

namespace RutinApp
{
    /// <summary>
    /// Clase encargada de configurar e inicializar
    /// la aplicación .NET MAUI.
    /// 
    /// Esta clase centraliza:
    /// - Configuración de fuentes.
    /// - Configuración de servicios.
    /// - Inyección de dependencias.
    /// - Configuración de Entity Framework.
    /// - Inicialización de la base de datos SQLite.
    /// - Registro de ViewModels y páginas.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Crea y configura la instancia principal
        /// de la aplicación MAUI.
        /// 
        /// También configura:
        /// - Base de datos SQLite.
        /// - Inyección de dependencias.
        /// - Migraciones automáticas.
        /// - Datos iniciales de usuario.
        /// </summary>
        /// <returns>
        /// Instancia configurada de <see cref="MauiApp"/>.
        /// </returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    // Registro de fuentes personalizadas
                    // utilizadas por la aplicación.
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Ruta donde se almacenará
            // la base de datos SQLite local.
            string dbPath =
                Path.Combine(FileSystem.AppDataDirectory, "app.db");

            // Configuración del contexto de base de datos
            // utilizando SQLite y Entity Framework Core.
            builder.Services.AddDbContextFactory<DataContext>(options =>
                options.UseSqlite($"Filename={dbPath}"));

            // Registro de ViewModels
            // utilizados en la aplicación.
            builder.Services.AddSingleton<EjercicioViewModel>();
            builder.Services.AddSingleton<RutinaViewModel>();
            builder.Services.AddSingleton<SesionViewModel>();
            builder.Services.AddTransient<HistorialViewModel>();

            // Registro de páginas
            // utilizadas en la navegación.
            builder.Services.AddTransient<EjerciciosPage>();
            builder.Services.AddTransient<AddEjercicioPage>();
            builder.Services.AddTransient<MisRutinasPage>();
            builder.Services.AddTransient<AddRutinaPage>();
            builder.Services.AddTransient<SesionPage>();
            builder.Services.AddTransient<HistorialPage>();

#if DEBUG
            // Habilita el registro de mensajes
            // de depuración en modo DEBUG.
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Inicialización de la base de datos.
            using (var scope = app.Services.CreateScope())
            {
                var factory =
                    scope.ServiceProvider
                    .GetRequiredService<IDbContextFactory<DataContext>>();

                using var db = factory.CreateDbContext();

                // Aplica automáticamente las migraciones pendientes.
                db.Database.Migrate();

                // Inserta un usuario por defecto
                // si la tabla de usuarios está vacía.
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