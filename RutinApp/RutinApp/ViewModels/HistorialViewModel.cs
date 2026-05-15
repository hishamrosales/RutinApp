using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using RutinApp.Models;
using RutinApp.Shared.Data;
using System.Collections.ObjectModel;

/// <summary>
/// Autores: Hisham Rosales, Debanhi Librado, Jesus Guerrero
/// Fecha: 15/05/2026
/// Versión: 1.5
/// </summary>
namespace RutinApp.ViewModels
{
    /// <summary>
    /// ViewModel encargado de administrar la lógica
    /// relacionada con el historial de rutinas realizadas.
    /// 
    /// Implementa el patrón MVVM utilizando
    /// <see cref="ObservableObject"/> para permitir
    /// la notificación automática de cambios en la interfaz.
    /// 
    /// Este ViewModel obtiene la información almacenada
    /// en la base de datos y la transforma en elementos
    /// visualizables dentro de la aplicación.
    /// </summary>
    public class HistorialViewModel : ObservableObject
    {
        /// <summary>
        /// Fábrica utilizada para crear instancias
        /// del contexto de base de datos.
        /// </summary>
        private readonly IDbContextFactory<DataContext> _contextFactory;

        /// <summary>
        /// Colección observable que almacena los elementos
        /// del historial mostrados en la interfaz.
        /// </summary>
        public ObservableCollection<HistorialItem> Historial { get; set; } = new();

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="HistorialViewModel"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// Fábrica utilizada para crear el contexto
        /// de acceso a datos.
        /// </param>
        public HistorialViewModel(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Obtiene el historial de rutinas almacenado
        /// en la base de datos y actualiza la colección observable.
        /// 
        /// Los registros se cargan ordenados de forma descendente
        /// según la fecha de realización.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        public async Task LoadHistorialAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var lista = await db.Historiales
                .AsNoTracking()
                .OrderByDescending(h => h.Fecha)
                .ToListAsync();

            Historial.Clear();

            foreach (var item in lista)
            {
                Historial.Add(new HistorialItem
                {
                    NombreRutina = item.Notas ?? "Rutina",
                    Fecha = item.Fecha,
                    Resumen = "Sesión completada"
                });
            }
        }
    }
}