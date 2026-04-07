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

namespace RutinApp.ViewModels
{
    public partial class RutinaViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        [ObservableProperty]
        private ObservableCollection<RutinaModel> _rutinas = [];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _nombreRutina = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private bool _isNombreValid = true;

        public bool IsFormValid => IsNombreValid;

        public RutinaViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        partial void OnNombreRutinaChanged(string value)
        {
            IsNombreValid = !string.IsNullOrWhiteSpace(value);
        }

        public async Task GetRutinasAsync()
        {
            Rutinas.Clear();
            var rutinasFromDb = await _dataContext.Rutinas
                .AsNoTracking()
                .ToListAsync();
            foreach (var rutina in rutinasFromDb)
                Rutinas.Add(rutina);
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
        public async Task DeleteRutina(RutinaModel rutina)
        {
            string userAnswer = await Shell.Current.DisplayActionSheetAsync(
                "¿Estás seguro de que quieres eliminar esta rutina?",
                "Cancelar", "Eliminar");
            if (userAnswer == "Cancelar") return;

            _dataContext.Rutinas.Remove(rutina);
            await _dataContext.SaveChangesAsync();
            Rutinas.Remove(rutina);
        }

        [RelayCommand]
        public async Task SaveRutina()
        {
            var foundRutina = await _dataContext.Rutinas
                .FirstOrDefaultAsync(r => r.NombreRutina == this.NombreRutina);

            if (foundRutina != null)
            {
                EditRutina(foundRutina);
                await _dataContext.SaveChangesAsync();
                await Shell.Current.DisplayAlertAsync("Éxito", "Rutina editada correctamente", "OK");
            }
            else
            {
                await CreateRutina();
            }

            await Shell.Current.GoToAsync("..");
        }

        private async Task CreateRutina()
        {
            var rutina = new RutinaModel
            {
                NombreRutina = this.NombreRutina,
                FechaCreacion = DateTime.Now
            };
            _dataContext.Rutinas.Add(rutina);
            await _dataContext.SaveChangesAsync();
        }

        private void EditRutina(RutinaModel rutina)
        {
            rutina.NombreRutina = this.NombreRutina;
        }

        public void LoadRutinaForEdition(RutinaModel rutina)
        {
            NombreRutina = rutina.NombreRutina;
        }
    }
}
