using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    public class DetalleLogModel
    {
        public int Id { get; set; }
        public int HistorialId { get; set; }
        public int EjercicioId { get; set; }
        public int PesoUtilizado { get; set; }
        public int RepsLogradas { get; set; }

        public HistorialEntrenamientoModel Historial { get; set; } = new();
        public EjercicioModel Ejercicio { get; set; } = new();
    }
}
