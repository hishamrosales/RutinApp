using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RutinApp.Shared.Data;
using RutinApp.Shared.Models;
using RutinApp.Views;
using System.Collections.ObjectModel;

namespace RutinApp.ViewModels
{
    public partial class RutinaViewModel : ObservableObject
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;
        private bool _esEdicion = false;
        private int _rutinaEditandoId = 0;

        [ObservableProperty]
        private ObservableCollection<RutinaModel> _rutinas = [];

        [ObservableProperty]
        private ObservableCollection<EjercicioSeleccionModel> _ejerciciosDisponibles = [];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _nombreRutina = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private bool _isNombreValid = true;


        private bool _haInteractuado = false;

        partial void OnNombreRutinaChanged(string value)
        {
            _haInteractuado = true;
            IsNombreValid = !string.IsNullOrWhiteSpace(value);
            OnPropertyChanged(nameof(IsFormValid));
        }

        public bool IsFormValid => !_haInteractuado || (IsNombreValid && !string.IsNullOrWhiteSpace(NombreRutina));

        public RutinaViewModel(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task GetRutinasAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();
            Rutinas.Clear();
            var lista = await db.Rutinas
                .AsNoTracking()
                .Include(r => r.RutinaEjercicios)
                .ToListAsync();
            foreach (var r in lista) Rutinas.Add(r);
        }

        public async Task GetEjerciciosDisponiblesAsync()
        {
            using var db = await _contextFactory.CreateDbContextAsync();
            EjerciciosDisponibles.Clear();
            var lista = await db.Ejercicios.AsNoTracking().ToListAsync();

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
                var seleccion = new EjercicioSeleccionModel { Ejercicio = e };
                if (idsEnRutina.Contains(e.Id))
                    seleccion.IsSelected = true;
                EjerciciosDisponibles.Add(seleccion);
            }
        }

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

        public void LoadRutinaForEdicion(RutinaModel rutina)
        {
            _esEdicion = true;
            _rutinaEditandoId = rutina.Id;
            _nombreRutina = rutina.NombreRutina;
            OnPropertyChanged(nameof(NombreRutina));
            IsNombreValid = true;
        }

        [RelayCommand]
        public async Task GoToAddPage()
        {
            await Shell.Current.GoToAsync(nameof(AddRutinaPage));
        }

        [RelayCommand]
        public async Task GoToEditPage(RutinaModel rutina)
        {
            await Shell.Current.GoToAsync(nameof(AddRutinaPage),
                new Dictionary<string, object> { ["Rutina"] = rutina });
        }

        [RelayCommand]
        public async Task ShowOptions(RutinaModel rutina)
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

        [RelayCommand]
        public async Task DeleteRutina(RutinaModel rutina)
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Eliminar rutina",
                $"¿Estás seguro de que quieres eliminar \"{rutina.NombreRutina}\"?",
                "Eliminar", "Cancelar");
            if (!confirm) return;

            using var db = await _contextFactory.CreateDbContextAsync();
            var toDelete = await db.Rutinas.FindAsync(rutina.Id);
            if (toDelete == null) return;
            db.Rutinas.Remove(toDelete);
            await db.SaveChangesAsync();
            Rutinas.Remove(rutina);
        }

        [RelayCommand]
        public async Task SaveRutina()
        {
            if (string.IsNullOrWhiteSpace(NombreRutina))
            {
                _haInteractuado = true; // 🔥 fuerza validación visual
                IsNombreValid = false;

                OnPropertyChanged(nameof(IsNombreValid));
                OnPropertyChanged(nameof(IsFormValid));

                return;
            }

            var seleccionados = EjerciciosDisponibles.Where(e => e.IsSelected).ToList();

            if (!seleccionados.Any())
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    "Debes seleccionar al menos un ejercicio",
                    "OK"
                );
                return;
            }

            if (_esEdicion)
            {
                using var db = await _contextFactory.CreateDbContextAsync();
                var rutina = await db.Rutinas.FindAsync(_rutinaEditandoId);
                if (rutina == null) return;

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

                int changes = await db.SaveChangesAsync();
                await Shell.Current.DisplayAlertAsync("Éxito", "Rutina editada correctamente", "OK");
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

        [RelayCommand]
        public async Task IniciarSesion(RutinaModel rutina)
        {
            await Shell.Current.GoToAsync($"//MisRutinasPage/{nameof(SesionPage)}",
                new Dictionary<string, object> { ["Rutina"] = rutina });
        }
    }
}