using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public ICollection<RutinaModel> Rutinas { get; set; } = [];
        public ICollection<HistorialEntrenamientoModel> Historiales { get; set; } = [];
    }
}
