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
    /// <summary>
    /// ViewModel encargado de administrar la lógica
    /// relacionada con las sesiones de entrenamiento.
    /// 
    /// Implementa el patrón MVVM utilizando
    /// CommunityToolkit.MVVM para:
    /// - Propiedades observables.
    /// - Comandos.
    /// - Notificación automática de cambios.
    /// 
    /// Este ViewModel permite:
    /// - Iniciar sesiones de entrenamiento.
    /// - Cargar ejercicios de una rutina.
    /// - Registrar progreso.
    /// - Marcar series completadas.
    /// - Finalizar sesiones.
    /// - Guardar historial de entrenamiento.
    /// </summary>
    public partial class SesionViewModel : ObservableObject
    {
        /// <summary>
        /// Fábrica utilizada para crear instancias
        /// del contexto de base de datos.
        /// </summary>
        private readonly IDbContextFactory<DataContext> _contextFactory;

        /// <summary>
        /// Referencia al ViewModel de rutinas utilizada
        /// para actualizar el progreso visual de la sesión.
        /// </summary>
        private readonly RutinaViewModel _rutinaViewModel;

        /// <summary>
        /// Lista interna que almacena los ejercicios
        /// pertenecientes a la rutina actual.
        /// </summary>
        private List<RutinaDetalleModel> _ejerciciosRutina = [];

        /// <summary>
        /// Índice del ejercicio actualmente mostrado.
        /// </summary>
        private int _ejercicioActualIndex = 0;

        /// <summary>
        /// Rutina actualmente activa durante la sesión.
        /// </summary>
        [ObservableProperty]
        private RutinaUIModel? _rutinaActiva;

        /// <summary>
        /// Nombre de la rutina en ejecución.
        /// </summary>
        [ObservableProperty]
        private string _nombreRutina = string.Empty;

        /// <summary>
        /// Nombre del ejercicio actual.
        /// </summary>
        [ObservableProperty]
        private string _nombreEjercicioActual = string.Empty;

        /// <summary>
        /// Grupo muscular asociado al ejercicio actual.
        /// </summary>
        [ObservableProperty]
        private string _grupoMuscularActual = string.Empty;

        /// <summary>
        /// Colección observable que almacena
        /// las series del ejercicio actual.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<SerieLogModel> _series = [];

        /// <summary>
        /// Número del ejercicio actual dentro de la rutina.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(EsUltimoEjercicio))]
        private int _ejercicioActualNumero = 1;

        /// <summary>
        /// Cantidad total de ejercicios de la rutina.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(EsUltimoEjercicio))]
        private int _totalEjercicios = 0;

        /// <summary>
        /// Valor del progreso actual de la sesión.
        /// 
        /// El rango esperado es:
        /// 0 = 0%
        /// 1 = 100%
        /// </summary>
        [ObservableProperty]
        private double _progreso = 0;

        /// <summary>
        /// Obtiene un valor que indica si el ejercicio actual
        /// corresponde al último ejercicio de la rutina.
        /// </summary>
        public bool EsUltimoEjercicio =>
            TotalEjercicios > 0 &&
            EjercicioActualNumero == TotalEjercicios;

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="SesionViewModel"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// Fábrica utilizada para crear instancias
        /// del contexto de base de datos.
        /// </param>
        /// <param name="rutinaViewModel">
        /// ViewModel utilizado para actualizar
        /// el progreso visual de la rutina activa.
        /// </param>
        public SesionViewModel(
            IDbContextFactory<DataContext> contextFactory,
            RutinaViewModel rutinaViewModel)
        {
            _contextFactory = contextFactory;
            _rutinaViewModel = rutinaViewModel;
        }

        /// <summary>
        /// Inicializa una sesión de entrenamiento
        /// utilizando la rutina seleccionada.
        /// 
        /// Carga todos los ejercicios asociados
        /// y reinicia el progreso de la sesión.
        /// </summary>
        /// <param name="rutina">
        /// Rutina utilizada para la sesión.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
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

            // Reinicia el progreso visual
            // de la rutina activa.
            if (_rutinaViewModel.RutinaActiva != null)
            {
                _rutinaViewModel.RutinaActiva.ProgresoSesion = 0;
            }

            CargarEjercicioActual();
        }

        /// <summary>
        /// Carga la información correspondiente
        /// al ejercicio actual de la rutina.
        /// 
        /// También actualiza:
        /// - El progreso.
        /// - El número de ejercicio.
        /// - Las series asociadas.
        /// </summary>
        private void CargarEjercicioActual()
        {
            if (_ejercicioActualIndex >= _ejerciciosRutina.Count)
                return;

            var detalle = _ejerciciosRutina[_ejercicioActualIndex];

            NombreEjercicioActual =
                detalle.Ejercicio?.Nombre ?? string.Empty;

            GrupoMuscularActual =
                detalle.Ejercicio?.GrupoMuscular ?? string.Empty;

            EjercicioActualNumero = _ejercicioActualIndex + 1;

            double progresoActual =
                TotalEjercicios > 0
                ? (double)_ejercicioActualIndex / TotalEjercicios
                : 0;

            Progreso = progresoActual;

            // Actualiza el progreso visual
            // mostrado en la pantalla de rutinas.
            if (_rutinaViewModel.RutinaActiva != null)
            {
                _rutinaViewModel.RutinaActiva.ProgresoSesion = progresoActual;
            }

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

        /// <summary>
        /// Marca una serie como completada.
        /// </summary>
        /// <param name="serie">
        /// Serie que será marcada como completada.
        /// </param>
        [RelayCommand]
        public void MarcarSerieCompletada(SerieLogModel serie)
        {
            serie.Completada = true;
        }

        /// <summary>
        /// Avanza al siguiente ejercicio de la rutina.
        /// 
        /// Si existen series pendientes,
        /// solicita confirmación al usuario.
        /// 
        /// Cuando se completan todos los ejercicios,
        /// finaliza la sesión automáticamente.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task SiguienteEjercicio()
        {
            var seriesSinCompletar =
                Series.Where(s => !s.Completada).ToList();

            if (seriesSinCompletar.Any())
            {
                bool continuar =
                    await Shell.Current.DisplayAlertAsync(
                        "Series incompletas",
                        "Aún tienes series sin completar. ¿Deseas continuar de todas formas?",
                        "Continuar",
                        "Cancelar");

                if (!continuar)
                    return;
            }

            _ejercicioActualIndex++;

            if (_ejercicioActualIndex >= _ejerciciosRutina.Count)
            {
                // Marca el progreso como completado.
                if (_rutinaViewModel.RutinaActiva != null)
                {
                    _rutinaViewModel.RutinaActiva.ProgresoSesion = 1.0;
                }

                await FinalizarSesion();

                return;
            }

            CargarEjercicioActual();
        }

        /// <summary>
        /// Finaliza la sesión de entrenamiento actual.
        /// 
        /// Registra la sesión en el historial
        /// y muestra un mensaje de confirmación.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
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