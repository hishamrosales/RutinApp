using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RutinApp.Shared.Data;
using RutinApp.Shared.Models;
using RutinApp.Views;
using System.Collections.ObjectModel;


namespace RutinApp.ViewModels
{
    public partial class EjercicioViewModel : ObservableObject
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;
        private bool _esEdicion = false;
        private int _ejercicioEditandoId = 0;

        [ObservableProperty]
        private ObservableCollection<EjercicioModel> _ejercicios = [];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _nombre = string.Empty;

        [ObservableProperty]
        private string _grupoMuscular = string.Empty;

        [ObservableProperty]
        private string _descripcion = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private bool _isNombreValid = true;



        private bool _haInteractuado = false;
        partial void OnNombreChanged(string value)
        {
            _haInteractuado = true;
            IsNombreValid = !string.IsNullOrWhiteSpace(value);
            OnPropertyChanged(nameof(IsFormValid));
        }
        public bool IsFormValid => !_haInteractuado || (IsNombreValid && !string.IsNullOrWhiteSpace(Nombre));

        
        public EjercicioViewModel(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task GetEjerciciosAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();
            Ejercicios.Clear();
            var lista = await db.Ejercicios.AsNoTracking().ToListAsync();
            foreach (var e in lista) Ejercicios.Add(e);
        }

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

        [RelayCommand]
        public async Task GoToAddPage()
        {
            await Shell.Current.GoToAsync(nameof(AddEjercicioPage));
        }

        [RelayCommand]
        public async Task GoToEditPage(EjercicioModel ejercicio)
        {
            await Shell.Current.GoToAsync(nameof(AddEjercicioPage),
                new Dictionary<string, object> { ["Ejercicio"] = ejercicio });
        }

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

        [RelayCommand]
        public async Task DeleteEjercicio(EjercicioModel ejercicio)
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Eliminar ejercicio",
                $"¿Estás seguro de que quieres eliminar \"{ejercicio.Nombre}\"?",
                "Eliminar", "Cancelar");
            if (!confirm) return;

            using var db = await _contextFactory.CreateDbContextAsync();
            var toDelete = await db.Ejercicios.FindAsync(ejercicio.Id);
            if (toDelete == null) return;
            db.Ejercicios.Remove(toDelete);
            await db.SaveChangesAsync();
            Ejercicios.Remove(ejercicio);
        }

        [RelayCommand]
        public async Task SaveEjercicio()
        {
            if (string.IsNullOrWhiteSpace(Nombre)) return;

            if (_esEdicion)
            {
                using var db = await _contextFactory.CreateDbContextAsync();
                var ejercicioToEdit = await db.Ejercicios.FindAsync(_ejercicioEditandoId);
                if (ejercicioToEdit == null) return;
                ejercicioToEdit.Nombre = Nombre;
                ejercicioToEdit.GrupoMuscular = GrupoMuscular;
                ejercicioToEdit.Descripcion = Descripcion;
                await db.SaveChangesAsync();
                await Shell.Current.DisplayAlertAsync("Éxito", "Ejercicio editado correctamente", "OK");
            }
            else
            {
                await CreateEjercicio();
            }

            await Shell.Current.GoToAsync("..");
        }

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