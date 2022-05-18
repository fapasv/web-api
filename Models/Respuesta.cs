﻿namespace webapi.Models
{
    public class Respuesta
    {
        public int IdEjercicio { get; set; }
        public int IdUsuario { get; set; }

        public string Valor { get; set; } = null!;

        
        public virtual Ejercicio EjercicioAsociado { get; set; } = null!;

    }
}
