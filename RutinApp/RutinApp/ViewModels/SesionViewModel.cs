using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RutinApp.Models;
using RutinApp.Shared.Data;
using RutinApp.Shared.Models;
using RutinApp.Views;
using System.Collections.ObjectModel;

namespace RutinApp.ViewModels
{
    public partial class SesionViewModel : ObservableObject
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;
        private readonly RutinaViewModel _rutinaViewModel;
        private List<RutinaDetalleModel> _ejerciciosRutina = [];
        private int _ejercicioActualIndex = 0;

        [ObservableProperty]
        private RutinaUIModel? _rutinaActiva;

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

        public SesionViewModel(IDbContextFactory<DataContext> contextFactory, RutinaViewModel rutinaViewModel)
        {
            _contextFactory = contextFactory;
            _rutinaViewModel = rutinaViewModel;
        }

        public async Task IniciarSesion(RutinaModel rutina)
        {
            NombreRutina = rutina.NombreRutina;
            _ejercicioActualIndex = 0;

            using var db = await _contextFactory.CreateDbContextAsync();
            _ejerciciosRutina = await db.RutinaEjercicios
                .AsNoTracking()
                .Include(re => re.Ejercicio)
                .Where(re => re.RutinaId == rutina.Id)
                .ToListAsync();

            TotalEjercicios = _ejerciciosRutina.Count;

            // Resetear el círculo de progreso al iniciar
            if (_rutinaViewModel.RutinaActiva != null)
                _rutinaViewModel.RutinaActiva.ProgresoSesion = 0;

            CargarEjercicioActual();
        }

        private void CargarEjercicioActual()
        {
            if (_ejercicioActualIndex >= _ejerciciosRutina.Count) return;

            var detalle = _ejerciciosRutina[_ejercicioActualIndex];
            NombreEjercicioActual = detalle.Ejercicio?.Nombre ?? string.Empty;
            GrupoMuscularActual = detalle.Ejercicio?.GrupoMuscular ?? string.Empty;
            EjercicioActualNumero = _ejercicioActualIndex + 1;

            double progresoActual = TotalEjercicios > 0
                ? (double)_ejercicioActualIndex / TotalEjercicios
                : 0;

            Progreso = progresoActual;

            // Actualiza el círculo de la tarjeta en MisRutinas
            if (_rutinaViewModel.RutinaActiva != null)
                _rutinaViewModel.RutinaActiva.ProgresoSesion = progresoActual;

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
                // Sesión terminada: círculo al 100%
                if (_rutinaViewModel.RutinaActiva != null)
                    _rutinaViewModel.RutinaActiva.ProgresoSesion = 1.0;

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
                Notas = NombreRutina
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