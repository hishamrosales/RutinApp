using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    public class RutinaModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreRutina { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public UsuarioModel Usuario { get; set; } = new();
        public ICollection<RutinaDetalleModel> RutinaEjercicios { get; set; } = [];
    }
}
