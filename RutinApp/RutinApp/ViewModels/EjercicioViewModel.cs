using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RutinApp.Shared.Data;
using RutinApp.Shared.Models;
using RutinApp.Views;
using System.Collections.ObjectModel;

namespace RutinApp.ViewModels
{
    /// <summary>
    /// ViewModel encargado de administrar la lógica de negocio
    /// relacionada con los ejercicios registrados en la aplicación.
    /// 
    /// Implementa el patrón MVVM utilizando CommunityToolkit.MVVM
    /// para el manejo de:
    /// - Propiedades observables.
    /// - Validaciones.
    /// - Comandos.
    /// - Notificación automática de cambios.
    /// 
    /// Este ViewModel permite:
    /// - Consultar ejercicios.
    /// - Crear ejercicios.
    /// - Editar ejercicios.
    /// - Eliminar ejercicios.
    /// - Validar formularios.
    /// - Navegar entre páginas.
    /// </summary>
    public partial class EjercicioViewModel : ObservableObject
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;

        private bool _esEdicion = false;
        private int _ejercicioEditandoId = 0;

        /// <summary>
        /// Colección observable que almacena los ejercicios
        /// mostrados en la interfaz.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<EjercicioModel> _ejercicios = [];

        /// <summary>
        /// Nombre del ejercicio.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _nombre = string.Empty;

        /// <summary>
        /// Grupo muscular asociado al ejercicio.
        /// </summary>
        [ObservableProperty]
        private string _grupoMuscular = string.Empty;

        /// <summary>
        /// Descripción adicional del ejercicio.
        /// </summary>
        [ObservableProperty]
        private string _descripcion = string.Empty;

        /// <summary>
        /// Indica si el nombre ingresado es válido.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private bool _isNombreValid = true;

        /// <summary>
        /// Indica si el grupo muscular ingresado es válido.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private bool _isGrupoMuscularValid = true;

        /// <summary>
        /// Método parcial ejecutado automáticamente cuando cambia
        /// la propiedad <see cref="GrupoMuscular"/>.
        /// 
        /// Actualiza el estado de validación del formulario.
        /// </summary>
        /// <param name="value">
        /// Nuevo valor asignado al grupo muscular.
        /// </param>
        partial void OnGrupoMuscularChanged(string value)
        {
            _haInteractuado = true;

            IsGrupoMuscularValid = !string.IsNullOrWhiteSpace(value);

            OnPropertyChanged(nameof(IsFormValid));
        }

        /// <summary>
        /// Indica si el usuario ya interactuó con el formulario.
        /// </summary>
        private bool _haInteractuado = false;

        /// <summary>
        /// Método parcial ejecutado automáticamente cuando cambia
        /// la propiedad <see cref="Nombre"/>.
        /// 
        /// Actualiza el estado de validación del formulario.
        /// </summary>
        /// <param name="value">
        /// Nuevo valor asignado al nombre del ejercicio.
        /// </param>
        partial void OnNombreChanged(string value)
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
        /// - El nombre no esté vacío.
        /// - El grupo muscular no esté vacío.
        /// - Ambas validaciones sean correctas.
        /// </summary>
        public bool IsFormValid =>
            !_haInteractuado ||
            (IsNombreValid &&
             IsGrupoMuscularValid &&
             !string.IsNullOrWhiteSpace(Nombre) &&
             !string.IsNullOrWhiteSpace(GrupoMuscular));

        /// <summary>
        /// Inicializa una nueva instancia del ViewModel
        /// <see cref="EjercicioViewModel"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// Fábrica utilizada para crear instancias del contexto
        /// de base de datos.
        /// </param>
        public EjercicioViewModel(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Obtiene todos los ejercicios registrados en la base de datos
        /// y actualiza la colección observable.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        public async Task GetEjerciciosAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            Ejercicios.Clear();

            var lista = await db.Ejercicios
                .AsNoTracking()
                .ToListAsync();

            foreach (var e in lista)
                Ejercicios.Add(e);
        }

        /// <summary>
        /// Restablece los valores del formulario a su estado inicial.
        /// 
        /// También desactiva el modo edición.
        /// </summary>
        public void LimpiarFormulario()
        {
            _haInteractuado = false;
            _esEdicion = false;
            _ejercicioEditandoId = 0;

            _nombre = string.Empty;
            OnPropertyChanged(nameof(Nombre));

            _grupoMuscular = string.Empty;
            OnPropertyChanged(nameof(GrupoMuscular));

            _descripcion = string.Empty;
            OnPropertyChanged(nameof(Descripcion));

            IsNombreValid = true;
        }

        /// <summary>
        /// Carga la información de un ejercicio en el formulario
        /// para permitir su edición.
        /// </summary>
        /// <param name="ejercicio">
        /// Ejercicio que será editado.
        /// </param>
        public void LoadEjercicioForEdicion(EjercicioModel ejercicio)
        {
            _esEdicion = true;

            _ejercicioEditandoId = ejercicio.Id;

            _nombre = ejercicio.Nombre;
            OnPropertyChanged(nameof(Nombre));

            _grupoMuscular = ejercicio.GrupoMuscular;
            OnPropertyChanged(nameof(GrupoMuscular));

            _descripcion = ejercicio.Descripcion;
            OnPropertyChanged(nameof(Descripcion));

            IsNombreValid = true;
        }

        /// <summary>
        /// Navega hacia la página para agregar un nuevo ejercicio.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task GoToAddPage()
        {
            await Shell.Current.GoToAsync(nameof(AddEjercicioPage));
        }

        /// <summary>
        /// Navega hacia la página de edición enviando
        /// el ejercicio seleccionado como parámetro.
        /// </summary>
        /// <param name="ejercicio">
        /// Ejercicio que será editado.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task GoToEditPage(EjercicioModel ejercicio)
        {
            await Shell.Current.GoToAsync(
                nameof(AddEjercicioPage),
                new Dictionary<string, object>
                {
                    ["Ejercicio"] = ejercicio
                });
        }

        /// <summary>
        /// Muestra un menú de opciones para el ejercicio seleccionado.
        /// 
        /// Las opciones disponibles son:
        /// - Editar.
        /// - Eliminar.
        /// </summary>
        /// <param name="ejercicio">
        /// Ejercicio seleccionado.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task ShowOptions(EjercicioModel ejercicio)
        {
            string userAnswer = await Shell.Current.DisplayActionSheetAsync(
                ejercicio.Nombre,
                "Cancelar",
                null,
                "Editar",
                "Eliminar");

            if (userAnswer == "Editar")
                await GoToEditPage(ejercicio);

            if (userAnswer == "Eliminar")
                await DeleteEjercicio(ejercicio);
        }

        /// <summary>
        /// Elimina un ejercicio de la base de datos
        /// previa confirmación del usuario.
        /// </summary>
        /// <param name="ejercicio">
        /// Ejercicio que será eliminado.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task DeleteEjercicio(EjercicioModel ejercicio)
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Eliminar ejercicio",
                $"¿Estás seguro de que quieres eliminar \"{ejercicio.Nombre}\"?",
                "Eliminar",
                "Cancelar");

            if (!confirm)
                return;

            using var db = await _contextFactory.CreateDbContextAsync();

            var toDelete = await db.Ejercicios.FindAsync(ejercicio.Id);

            if (toDelete == null)
                return;

            db.Ejercicios.Remove(toDelete);

            await db.SaveChangesAsync();

            Ejercicios.Remove(ejercicio);
        }

        /// <summary>
        /// Guarda un ejercicio en la base de datos.
        /// 
        /// Si el formulario se encuentra en modo edición,
        /// actualiza el ejercicio existente.
        /// 
        /// En caso contrario, crea un nuevo ejercicio.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        [RelayCommand]
        public async Task SaveEjercicio()
        {
            _haInteractuado = true;

            IsNombreValid = !string.IsNullOrWhiteSpace(Nombre);
            IsGrupoMuscularValid = !string.IsNullOrWhiteSpace(GrupoMuscular);

            OnPropertyChanged(nameof(IsNombreValid));
            OnPropertyChanged(nameof(IsGrupoMuscularValid));
            OnPropertyChanged(nameof(IsFormValid));

            if (!IsNombreValid || !IsGrupoMuscularValid)
                return;

            if (_esEdicion)
            {
                using var db = await _contextFactory.CreateDbContextAsync();

                var ejercicioToEdit =
                    await db.Ejercicios.FindAsync(_ejercicioEditandoId);

                if (ejercicioToEdit == null)
                    return;

                ejercicioToEdit.Nombre = Nombre;
                ejercicioToEdit.GrupoMuscular = GrupoMuscular;
                ejercicioToEdit.Descripcion = Descripcion;

                await db.SaveChangesAsync();

                await Shell.Current.DisplayAlertAsync(
                    "Éxito",
                    "Ejercicio editado correctamente",
                    "OK");
            }
            else
            {
                await CreateEjercicio();
            }

            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Crea un nuevo ejercicio y lo guarda
        /// en la base de datos.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica.
        /// </returns>
        private async Task CreateEjercicio()
        {
            using var db = await _contextFactory.CreateDbContextAsync();

            db.Ejercicios.Add(new EjercicioModel
            {
                Nombre = Nombre,
                GrupoMuscular = GrupoMuscular,
                Descripcion = Descripcion
            });

            await db.SaveChangesAsync();
        }
    }
}