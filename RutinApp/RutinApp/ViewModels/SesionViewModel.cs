using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RutinApp.Shared.Data;
using RutinApp.Shared.Models;
using System.Collections.ObjectModel;
using RutinApp.Models;
using RutinApp.ViewModels;

namespace RutinApp.ViewModels
{
    public partial class SesionViewModel : ObservableObject
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;
        private RutinaModel _rutina;
        private List<RutinaDetalleModel> _ejerciciosRutina = [];
        private int _ejercicioActualIndex = 0;

        [ObservableProperty]
        private string _nombreRutina = string.Empty;

        [ObservableProperty]
        private string _nombreEjercicioActual = string.Empty;

        [ObservableProperty]
        private string _grupoMuscularActual = string.Empty;

        [ObservableProperty]
        private ObservableCollection<SerieLogModel> _series = [];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(EsUltimoEjercicio))]
        private int _ejercicioActualNumero = 1;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(EsUltimoEjercicio))]
        private int _totalEjercicios = 0;

        [ObservableProperty]
        private double _progreso = 0;

        public bool EsUltimoEjercicio => TotalEjercicios > 0 && EjercicioActualNumero == TotalEjercicios;

        public SesionViewModel(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task IniciarSesion(RutinaModel rutina)
        {
            _rutina = rutina;
            NombreRutina = rutina.NombreRutina;
            _ejercicioActualIndex = 0;

            using var db = await _contextFactory.CreateDbContextAsync();
            _ejerciciosRutina = await db.RutinaEjercicios
                .AsNoTracking()
                .Include(re => re.Ejercicio)
                .Where(re => re.RutinaId == rutina.Id)
                .ToListAsync();

            TotalEjercicios = _ejerciciosRutina.Count;
            CargarEjercicioActual();
        }

        private void CargarEjercicioActual()
        {
            if (_ejercicioActualIndex >= _ejerciciosRutina.Count) return;

            var detalle = _ejerciciosRutina[_ejercicioActualIndex];
            NombreEjercicioActual = detalle.Ejercicio?.Nombre ?? string.Empty;
            GrupoMuscularActual = detalle.Ejercicio?.GrupoMuscular ?? string.Empty;
            EjercicioActualNumero = _ejercicioActualIndex + 1;
            Progreso = (double)_ejercicioActualIndex / TotalEjercicios;

            Series.Clear();
            for (int i = 1; i <= detalle.Series; i++)
            {
                Series.Add(new SerieLogModel
                {
                    NumeroSerie = i,
                    RepsLogradas = detalle.Repeticiones
                });
            }
        }

        [RelayCommand]
        public void MarcarSerieCompletada(SerieLogModel serie)
        {
            serie.Completada = true;
        }

        [RelayCommand]
        public async Task SiguienteEjercicio()
        {
            var seriesSinCompletar = Series.Where(s => !s.Completada).ToList();
            if (seriesSinCompletar.Any())
            {
                bool continuar = await Shell.Current.DisplayAlertAsync(
                    "Series incompletas",
                    "Aún tienes series sin completar. ¿Deseas continuar de todas formas?",
                    "Continuar", "Cancelar");
                if (!continuar) return;
            }

            _ejercicioActualIndex++;

            if (_ejercicioActualIndex >= _ejerciciosRutina.Count)
            {
                await FinalizarSesion();
                return;
            }

            CargarEjercicioActual();
        }
        private async Task FinalizarSesion()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var historial = new HistorialEntrenamientoModel
            {
                UsuarioId = 1,
                Fecha = DateTime.Now,
                Notas = NombreRutina // 🔥 AQUÍ guardas el nombre de la rutina
            };

            db.Historiales.Add(historial);
            await db.SaveChangesAsync();

            await Shell.Current.DisplayAlertAsync(
                "¡Sesión completada!",
                $"Has completado tu rutina {NombreRutina}. ¡Buen trabajo!",
                "OK");

            await Shell.Current.GoToAsync("//MisRutinasPage");
        }
    }
}