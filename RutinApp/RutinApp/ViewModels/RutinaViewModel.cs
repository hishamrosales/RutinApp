using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RutinApp.Shared.Data;
using RutinApp.Shared.Models;
using RutinApp.Views;
using System.Collections.ObjectModel;
using RutinApp.Models;

namespace RutinApp.ViewModels
{
    /// <summary>
    /// ViewModel encargado de administrar la lógica de negocio
    /// relacionada con las rutinas de entrenamiento.
    /// 
    /// Implementa el patrón MVVM utilizando CommunityToolkit.MVVM
    /// para el manejo de:
    /// - Propiedades observables.
    /// - Comandos.
    /// - Validaciones.
    /// - Notificación automática de cambios.
    /// 
    /// Este ViewModel permite:
    /// - Consultar rutinas.
    /// - Crear rutinas.
    /// - Editar rutinas.
    /// - Eliminar rutinas.
    /// - Gestionar ejercicios asociados.
    /// - Iniciar sesiones de entrenamiento.
    /// </summary>
    public partial class RutinaViewModel : ObservableObject
    {
        /// <summary>
        /// Fábrica utilizada para crear instancias
        /// del contexto de base de datos.
        /// </summary>
        private readonly IDbContextFactory<DataContext> _contextFactory;

        /// <summary>
        /// Indica si el formulario se encuentra
        /// en modo edición.
        /// </summary>
        private bool _esEdicion = false;

        /// <summary>
        /// Identificador de la rutina que se encuentra
        /// en edición actualmente.
        /// </summary>
        private int _rutinaEditandoId = 0;

        /// <summary>
        /// Obtiene la rutina actualmente activa
        /// durante una sesión de entrenamiento.
        /// </summary>
        public RutinaUIModel? RutinaActiva { get; private set; }

        /// <summary>
        /// Colección observable que almacena
        /// las rutinas disponibles en la aplicación.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<RutinaUIModel> _rutinas = [];

        /// <summary>
        /// Colección observable que almacena
        /// los ejercicios disponibles para agregar
        /// a una rutina.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<EjercicioSeleccionModel> _ejerciciosDisponibles = [];

        /// <summary>
        /// Nombre de la rutina.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _nombreRutina = string.Empty;

        /// <summary>
        /// Indica si el nombre de la rutina es válido.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private bool _isNombreValid = true;

        /// <summary>
        /// Indica si el usuario ya interactuó
        /// con el formulario.
        /// </summary>
        private bool _haInteractuado = false;

        /// <summary>
        /// Método parcial ejecutado automáticamente
        /// cuando cambia la propiedad <see cref="NombreRutina"/>.
        /// 
        /// Actualiza el estado de validación del formulario.
        /// </summary>
        /// <param name="value">
        /// Nuevo valor asignado al nombre de la rutina.
        /// </param>
        partial void OnNombreRutinaChanged(string value)
        {
            _haInteractuado = true;

            IsNombreValid = !string.IsNullOrWhiteSpace(value);

            OnPropertyChanged(nameof(IsFormValid));
        }

        /// <summary>
        /// Obtiene un valor que indica si el formulario es válido.
        /// 
        /// El formulario será válido únicamente cuando:
        /// - El usuario haya interactuado con él.
        /// - El nombre de la rutina no esté vacío.
        /// </summary>
        public bool IsFormValid =>
            !_haInteractuado ||
            (IsNombreValid &&
             !string.IsNullOrWhiteSpace(NombreRutina));

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="RutinaViewModel"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// Fábrica utilizada para crear instancias
        /// del contexto de base de datos.
        /// </param>
        public RutinaViewModel(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Obtiene las rutinas registradas en la base de datos
        /// y actualiza la colección observable.
        /// 
        /// También preserva el progreso de sesiones activas
        /// reutilizando instancias existentes.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        public async Task GetRutinasAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            var lista = await db.Rutinas
                .AsNoTracking()
                .Include(r => r.RutinaEjercicios)
                .ToListAsync();

            // Reutiliza instancias existentes
            // para conservar el progreso actual.
            var existentes = Rutinas.ToDictionary(r => r.Id);

            var nuevas = new ObservableCollection<RutinaUIModel>();

            foreach (var r in lista)
            {
                if (existentes.TryGetValue(r.Id, out var uiExistente))
                    nuevas.Add(uiExistente);
                else
                    nuevas.Add(new RutinaUIModel(r));
            }

            Rutinas = nuevas;

            // Reasigna la rutina activa
            // utilizando la instancia actualizada.
            if (RutinaActiva != null)
            {
                RutinaActiva =
                    Rutinas.FirstOrDefault(r => r.Id == RutinaActiva.Id)
                    ?? RutinaActiva;
            }
        }

        /// <summary>
        /// Obtiene los ejercicios disponibles desde la base de datos
        /// y actualiza la colección observable.
        /// 
        /// Si la rutina se encuentra en modo edición,
        /// marca automáticamente los ejercicios ya asociados.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        public async Task GetEjerciciosDisponiblesAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            EjerciciosDisponibles.Clear();

            var lista = await db.Ejercicios
                .AsNoTracking()
                .ToListAsync();

            List<int> idsEnRutina = [];

            if (_esEdicion)
            {
                idsEnRutina = await db.RutinaEjercicios
                    .Where(re => re.RutinaId == _rutinaEditandoId)
                    .Select(re => re.EjercicioId)
                    .ToListAsync();
            }

            foreach (var e in lista)
            {
                var seleccion = new EjercicioSeleccionModel
                {
                    Ejercicio = e
                };

                if (idsEnRutina.Contains(e.Id))
                    seleccion.IsSelected = true;

                EjerciciosDisponibles.Add(seleccion);
            }
        }

        /// <summary>
        /// Restablece el formulario a su estado inicial.
        /// 
        /// También desactiva el modo edición.
        /// </summary>
        public void LimpiarFormulario()
        {
            _haInteractuado = false;
            _esEdicion = false;
            _rutinaEditandoId = 0;

            _nombreRutina = string.Empty;

            OnPropertyChanged(nameof(NombreRutina));

            _isNombreValid = true;

            OnPropertyChanged(nameof(IsNombreValid));
            OnPropertyChanged(nameof(IsFormValid));
        }

        /// <summary>
        /// Carga una rutina en el formulario
        /// para permitir su edición.
        /// </summary>
        /// <param name="rutina">
        /// Rutina que será editada.
        /// </param>
        public void LoadRutinaForEdicion(RutinaModel rutina)
        {
            _esEdicion = true;

            _rutinaEditandoId = rutina.Id;

            _nombreRutina = rutina.NombreRutina;

            OnPropertyChanged(nameof(NombreRutina));

            IsNombreValid = true;
        }

        /// <summary>
        /// Navega hacia la página para agregar
        /// una nueva rutina.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task GoToAddPage()
        {
            await Shell.Current.GoToAsync(nameof(AddRutinaPage));
        }

        /// <summary>
        /// Navega hacia la página de edición
        /// enviando la rutina seleccionada.
        /// </summary>
        /// <param name="rutina">
        /// Rutina que será editada.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task GoToEditPage(RutinaUIModel rutina)
        {
            await Shell.Current.GoToAsync(
                nameof(AddRutinaPage),
                new Dictionary<string, object>
                {
                    ["Rutina"] = rutina.Rutina
                });
        }

        /// <summary>
        /// Muestra un menú de opciones para la rutina seleccionada.
        /// 
        /// Las opciones disponibles son:
        /// - Editar.
        /// - Eliminar.
        /// </summary>
        /// <param name="rutina">
        /// Rutina seleccionada.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task ShowOptions(RutinaUIModel rutina)
        {
            string userAnswer = await Shell.Current.DisplayActionSheetAsync(
                rutina.NombreRutina,
                "Cancelar",
                null,
                "Editar",
                "Eliminar");

            if (userAnswer == "Editar")
                await GoToEditPage(rutina);

            if (userAnswer == "Eliminar")
                await DeleteRutina(rutina);
        }

        /// <summary>
        /// Elimina una rutina de la base de datos
        /// previa confirmación del usuario.
        /// </summary>
        /// <param name="rutina">
        /// Rutina que será eliminada.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task DeleteRutina(RutinaUIModel rutina)
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Eliminar rutina",
                $"¿Estás seguro de que quieres eliminar \"{rutina.NombreRutina}\"?",
                "Eliminar",
                "Cancelar");

            if (!confirm)
                return;

            using var db = await _contextFactory.CreateDbContextAsync();

            var toDelete = await db.Rutinas.FindAsync(rutina.Id);

            if (toDelete == null)
                return;

            db.Rutinas.Remove(toDelete);

            await db.SaveChangesAsync();

            Rutinas.Remove(rutina);
        }

        /// <summary>
        /// Guarda una rutina en la base de datos.
        /// 
        /// Si el formulario está en modo edición,
        /// actualiza la rutina existente.
        /// 
        /// En caso contrario, crea una nueva rutina.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task SaveRutina()
        {
            if (string.IsNullOrWhiteSpace(NombreRutina))
            {
                _haInteractuado = true;

                IsNombreValid = false;

                OnPropertyChanged(nameof(IsNombreValid));
                OnPropertyChanged(nameof(IsFormValid));

                return;
            }

            var seleccionados =
                EjerciciosDisponibles
                .Where(e => e.IsSelected)
                .ToList();

            if (!seleccionados.Any())
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    "Debes seleccionar al menos un ejercicio",
                    "OK");

                return;
            }

            if (_esEdicion)
            {
                using var db = await _contextFactory.CreateDbContextAsync();

                var rutina =
                    await db.Rutinas.FindAsync(_rutinaEditandoId);

                if (rutina == null)
                    return;

                rutina.NombreRutina = NombreRutina;

                var anteriores = await db.RutinaEjercicios
                    .Where(re => re.RutinaId == rutina.Id)
                    .ToListAsync();

                db.RutinaEjercicios.RemoveRange(anteriores);

                await db.SaveChangesAsync();

                foreach (var item in seleccionados)
                {
                    var detalle = new RutinaDetalleModel
                    {
                        RutinaId = rutina.Id,
                        EjercicioId = item.Ejercicio.Id,
                        Series = item.Series,
                        Repeticiones = item.Repeticiones
                    };

                    db.Entry(detalle).State = EntityState.Added;
                }

                await db.SaveChangesAsync();

                await Shell.Current.DisplayAlertAsync(
                    "Éxito",
                    "Rutina editada correctamente",
                    "OK");
            }
            else
            {
                using var db = await _contextFactory.CreateDbContextAsync();

                var rutina = new RutinaModel
                {
                    NombreRutina = NombreRutina,
                    FechaCreacion = DateTime.Now,
                    UsuarioId = 1
                };

                db.Rutinas.Add(rutina);

                await db.SaveChangesAsync();

                foreach (var item in seleccionados)
                {
                    var detalle = new RutinaDetalleModel
                    {
                        RutinaId = rutina.Id,
                        EjercicioId = item.Ejercicio.Id,
                        Series = item.Series,
                        Repeticiones = item.Repeticiones
                    };

                    db.Entry(detalle).State = EntityState.Added;
                }

                if (seleccionados.Any())
                    await db.SaveChangesAsync();
            }

            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Inicia una sesión de entrenamiento
        /// utilizando la rutina seleccionada.
        /// </summary>
        /// <param name="rutina">
        /// Rutina seleccionada para la sesión.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task IniciarSesion(RutinaUIModel rutina)
        {
            RutinaActiva = rutina;

            await Shell.Current.GoToAsync(
                nameof(SesionPage),
                new Dictionary<string, object>
                {
                    ["Rutina"] = rutina.Rutina
                });
        }
    }
}