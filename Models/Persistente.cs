using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoyalVillaApi.Models
{
    public abstract class Persistente
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [Required]
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
        [Required]
        public bool Activo { get; set; } = true;


        public void Desactivar()
        {
            this.Activo = false;
            this.FechaActualizacion = DateTime.Now;
        }

        public void Activar()
        {
            this.Activo = true;
            this.FechaActualizacion = DateTime.Now;
        }
    }
}