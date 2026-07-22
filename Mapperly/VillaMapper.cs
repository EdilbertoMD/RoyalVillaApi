using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riok.Mapperly.Abstractions;
using RoyalVillaApi.Models;
using RoyalVillaApi.Models.Dtos;

namespace RoyalVillaApi.Mapperly
{
    [Mapper]
    public partial class VillaMapper
    {
        #region Mapeo directo
        // Mapeo simple automático
        public partial VillaResponseDto ToDto(Villa villa);
        /*    
        // Ignorar una propiedad
        [MapperIgnoreSource(nameof(Villa.ImagenUrl))]
        public partial VillaResponseDto ToDtoSinImagen(Villa villa);

        // Mapear propiedades con distinto nombre
        [MapProperty(nameof(Villa.Nombre), nameof(VillaResponseDto.Nombre))]
        public partial VillaResponseDto ToDtoConNombre(Villa villa);*/

        // Listas
        public partial List<VillaResponseDto> ToDtoList(List<Villa> villas);
        #endregion

        #region Mapeo inverso
        // Mapeo inverso: de dto a entity
        public partial Villa ToEntity(VillaCreateRequestDto dto);

        // Mapeo inverso: de dto a entity
        public partial Villa ToEntity(VillaUpdateRequestDto dto);

        // Define un método void para mapear sobre una entidad existente
        public partial void UpdateVilla(VillaUpdateRequestDto dto, Villa villa);
        #endregion
    }
}