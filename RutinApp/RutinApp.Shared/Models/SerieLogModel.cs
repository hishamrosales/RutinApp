using CommunityToolkit.Mvvm.ComponentModel;

namespace RutinApp.Shared.Models
{
    public partial class SerieLogModel : ObservableObject
    {
        public int NumeroSerie { get; set; }

        [ObservableProperty]
        private int _pesoUtilizado = 0;

        [ObservableProperty]
        private int _repsLogradas = 0;

        [ObservableProperty]
        private bool _completada = false;
    }
}