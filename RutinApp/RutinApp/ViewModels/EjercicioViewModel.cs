using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RutinApp.Shared.Data;
using RutinApp.Shared.Models;
using RutinApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RutinApp.ViewModels
{
    public partial class EjercicioViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

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

        public bool IsFormValid => IsNombreValid;

        public EjercicioViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        partial void OnNombreChanged(string value)
        {
            IsNombreValid = !string.IsNullOrWhiteSpace(value);
        }

        public async Task GetEjerciciosAsync()
        {
            Ejercicios.Clear();
            var ejerciciosFromDb = await _dataContext.Ejercicios
                .AsNoTracking()
                .ToListAsync();
            foreach (var ejercicio in ejerciciosFromDb)
                Ejercicios.Add(ejercicio);
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
        public async Task DeleteEjercicio(EjercicioModel ejercicio)
        {
            string userAnswer = await Shell.Current.DisplayActionSheetAsync(
                "¿Estás seguro de que quieres eliminar este ejercicio?",
                "Cancelar", "Eliminar");
            if (userAnswer == "Cancelar") return;

            _dataContext.Ejercicios.Remove(ejercicio);
            await _dataContext.SaveChangesAsync();
            Ejercicios.Remove(ejercicio);
        }

        [RelayCommand]
        public async Task SaveEjercicio()
        {
            var foundEjercicio = await _dataContext.Ejercicios
                .FirstOrDefaultAsync(e => e.Nombre == this.Nombre);

            if (foundEjercicio != null)
            {
                EditEjercicio(foundEjercicio);
                await _dataContext.SaveChangesAsync();
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
            var ejercicio = new EjercicioModel
            {
                Nombre = this.Nombre,
                GrupoMuscular = this.GrupoMuscular,
                Descripcion = this.Descripcion
            };
            _dataContext.Ejercicios.Add(ejercicio);
            await _dataContext.SaveChangesAsync();
        }

        private void EditEjercicio(EjercicioModel ejercicio)
        {
            ejercicio.Nombre = this.Nombre;
            ejercicio.GrupoMuscular = this.GrupoMuscular;
            ejercicio.Descripcion = this.Descripcion;
        }

        public void LoadEjercicioForEdition(EjercicioModel ejercicio)
        {
            Nombre = ejercicio.Nombre;
            GrupoMuscular = ejercicio.GrupoMuscular;
            Descripcion = ejercicio.Descripcion;
        }
    }
}
