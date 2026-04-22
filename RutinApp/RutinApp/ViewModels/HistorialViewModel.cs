using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using RutinApp.Models;
using RutinApp.Shared.Data;
using System.Collections.ObjectModel;

namespace RutinApp.ViewModels;

public class HistorialViewModel : ObservableObject
{
    private readonly IDbContextFactory<DataContext> _contextFactory;

    public ObservableCollection<HistorialItem> Historial { get; set; } = new();

    public HistorialViewModel(IDbContextFactory<DataContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

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