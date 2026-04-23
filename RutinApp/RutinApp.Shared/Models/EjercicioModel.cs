using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    public class EjercicioModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string GrupoMuscular { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public ICollection<RutinaDetalleModel> RutinaEjercicios { get; set; } = [];
        public ICollection<DetalleLogModel> DetalleLogs { get; set; } = [];
    }
}
