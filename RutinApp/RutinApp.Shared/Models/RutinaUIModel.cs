using CommunityToolkit.Mvvm.ComponentModel;
using RutinApp.Shared.Models;

namespace RutinApp.Models
{
    /// <summary>
    /// Wrapper de RutinaModel con propiedades extra solo para la UI.
    /// No toca la base de datos.
    /// </summary>
    public partial class RutinaUIModel : ObservableObject
    {
        public RutinaModel Rutina { get; }

        // Delegamos las propiedades que usa el XAML
        public int Id => Rutina.Id;
        public string NombreRutina => Rutina.NombreRutina;
        public ICollection<RutinaDetalleModel> RutinaEjercicios => Rutina.RutinaEjercicios;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PorcentajeSesionTexto))]
        private double _progresoSesion = 0;

        public string PorcentajeSesionTexto => $"{(int)(ProgresoSesion * 100)}%";

        public RutinaUIModel(RutinaModel rutina)
        {
            Rutina = rutina;
        }
    }
}