using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    public class HistorialEntrenamientoModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Notas { get; set; } = string.Empty;

        public UsuarioModel? Usuario { get; set; }
        public ICollection<DetalleLogModel> DetalleLogs { get; set; } = [];
    }
}
