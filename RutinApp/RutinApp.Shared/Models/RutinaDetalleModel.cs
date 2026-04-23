using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    public class RutinaDetalleModel
    {
        public int RutinaId { get; set; }
        public int EjercicioId { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }

        public RutinaModel? Rutina { get; set; }
        public EjercicioModel? Ejercicio { get; set; }
    }
}
