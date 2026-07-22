using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyalVillaApi.Models.Dtos
{
    public class VillaUpdateRequestDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int MetrosCuadrados { get; set; }
        public string? ImagenUrl { get; set; }
    }
}