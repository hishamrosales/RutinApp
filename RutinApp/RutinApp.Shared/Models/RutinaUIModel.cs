using CommunityToolkit.Mvvm.ComponentModel;
using RutinApp.Shared.Models;

namespace RutinApp.Models
{
    /// <summary>
    /// Modelo de interfaz de usuario utilizado para extender
    /// la funcionalidad de <see cref="RutinaModel"/>
    /// exclusivamente en la capa visual.
    /// 
    /// Esta clase funciona como un wrapper del modelo principal,
    /// agregando propiedades específicas para la interfaz
    /// sin modificar la estructura de la base de datos.
    /// 
    /// Implementa el patrón MVVM mediante
    /// <see cref="ObservableObject"/> para permitir
    /// la actualización automática de la interfaz.
    /// </summary>
    public partial class RutinaUIModel : ObservableObject
    {
        /// <summary>
        /// Modelo original de la rutina asociado
        /// a la interfaz de usuario.
        /// </summary>
        public RutinaModel Rutina { get; }

        /// <summary>
        /// Obtiene el identificador de la rutina.
        /// 
        /// Esta propiedad delega el valor
        /// desde <see cref="Rutina"/>.
        /// </summary>
        public int Id => Rutina.Id;

        /// <summary>
        /// Obtiene el nombre de la rutina.
        /// 
        /// Esta propiedad delega el valor
        /// desde <see cref="Rutina"/>.
        /// </summary>
        public string NombreRutina => Rutina.NombreRutina;

        /// <summary>
        /// Obtiene la colección de ejercicios
        /// asociados a la rutina.
        /// 
        /// Esta propiedad delega el valor
        /// desde <see cref="Rutina"/>.
        /// </summary>
        public ICollection<RutinaDetalleModel> RutinaEjercicios =>
            Rutina.RutinaEjercicios;

        /// <summary>
        /// Representa el progreso actual de la sesión
        /// asociado a la rutina.
        /// 
        /// El rango esperado es:
        /// 0 = 0%
        /// 1 = 100%
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PorcentajeSesionTexto))]
        private double _progresoSesion = 0;

        /// <summary>
        /// Obtiene el porcentaje de progreso
        /// formateado como texto para mostrar
        /// en la interfaz de usuario.
        /// 
        /// Ejemplo:
        /// "75%"
        /// </summary>
        public string PorcentajeSesionTexto =>
            $"{(int)(ProgresoSesion * 100)}%";

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="RutinaUIModel"/>.
        /// </summary>
        /// <param name="rutina">
        /// Modelo de rutina utilizado como base
        /// para la interfaz de usuario.
        /// </param>
        public RutinaUIModel(RutinaModel rutina)
        {
            Rutina = rutina;
        }
    }
}