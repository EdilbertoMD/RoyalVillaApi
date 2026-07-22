using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVillaApi.Data;
using RoyalVillaApi.Mapperly;
using RoyalVillaApi.Models.Dtos;

namespace RoyalVillaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VillaController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly VillaMapper _mapper;

        public VillaController(ApplicationDbContext context, VillaMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaResponseDto>>>> GetAllVillas()
        {
            try
            {
                // Obtener todas las villas
                var villas = await _context.Villas.ToListAsync();
                // Si no hay villas
                if (!villas.Any())
                    // Retorna no content
                    return Ok(ApiResponse<IEnumerable<VillaResponseDto>>.NoContent("No se encontraron villas registradas."));
                // Mapea la lista de villas a la lista de dtos
                var villasDto = _mapper.ToDtoList(villas);
                // Crear response
                var response = ApiResponse<IEnumerable<VillaResponseDto>>.Ok("Villas obtenidas exitosamente", villasDto);
                // Retorna las villas en formato dto
                return Ok(response);
            }
            catch(Exception ex)
            {
                // Crear response
                var response = ApiResponse<object>.Error(ex, "Error al obtener las villas");
                // Retornar error si algo sale mal
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<VillaResponseDto>>> GetVillaById(int id)
        {
            try
            {
                // Validar que el id sea mayor a 0
                if (id <= 0)
                    return BadRequest(ApiResponse<object>.BadRequest("El ID debe ser mayor a 0"));
                // Obtener la villa
                var villa = await _context.Villas.FindAsync(id);
                // Si no hay villa
                if (villa == null)
                    // Retorna no content
                    return NotFound(ApiResponse<object>.NotFound($"La villa con ID {id} no existe"));
                // Mapea la villa a dto
                var villaDto = _mapper.ToDto(villa);
                // Crear response
                var response = ApiResponse<VillaResponseDto>.Ok("Villa obtenida exitosamente", villaDto);
                // Retorna la villa en formato dto
                return Ok(response);
            }
            catch(Exception ex)
            {
                // Crear response
                var response = ApiResponse<object>.Error(ex, "Error al obtener la villa");
                // Retornar error si algo sale mal
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // CREAR VILLA
        [HttpPost]
        public async Task<ActionResult<ApiResponse<VillaResponseDto>>> CreateVilla([FromBody] VillaCreateRequestDto villaCreateDto)
        {
            try
            {
                // Mapear el dto a entity
                var villa = _mapper.ToEntity(villaCreateDto);
                // Validamos si es que ya existe una villa con el mismo nombre
                var existingVilla = await _context.Villas.FirstOrDefaultAsync(
                    v => v.Nombre.ToLower() == villa.Nombre.ToLower());
                // Si existe, retornamos bad request
                if (existingVilla != null)
                    return Conflict(ApiResponse<object>.Conflict($"La villa con nombre {villa.Nombre} ya existe"));
                // Seteo de fechas de creacion 
                villa.FechaCreacion = DateTime.Now;
                villa.FechaActualizacion = DateTime.Now;
                // Agregar la villa a la base de datos
                _context.Villas.Add(villa);
                await _context.SaveChangesAsync();
                // Mapear la villa a dto para retornar
                var villaDto = _mapper.ToDto(villa);
                // Crear response
                var response = ApiResponse<VillaResponseDto>.CreatedAt("Villa creada exitosamente", villaDto);
                // Retornar la villa creada con status 201 Created
                // Location header apuntando al nuevo recurso
                return CreatedAtAction(nameof(GetVillaById), new { id = villaDto.Id }, response);
            }
            catch(Exception ex)
            {
                // Crear response
                var response = ApiResponse<object>.Error(ex, "Error al crear la villa");
                // Retornar error si algo sale mal
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        // ACTUALIZAR VILLA
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<VillaResponseDto>>> UpdateVilla(int id, [FromBody] VillaUpdateRequestDto villaUpdateDto)
        {
            try
            {
                // Verificar si la villa existe
                var existingVilla = await _context.Villas.FindAsync(id);
                if (existingVilla == null)
                    return NotFound(ApiResponse<object>.NotFound($"La villa con ID {id} no existe"));
                // Validar si ya existe una villa con el mismo nombre, diferente a la que estamos actualizando
                var existingVillaWithSameName = await _context.Villas.FirstOrDefaultAsync(
                    v => v.Nombre.ToLower() == villaUpdateDto.Nombre.ToLower() && v.Id != id);
                // Si existe, retornamos bad request
                if (existingVillaWithSameName != null)
                    return Conflict(ApiResponse<object>.Conflict($"La villa con nombre {villaUpdateDto.Nombre} ya existe"));
                // Mapear el dto a entity
                _mapper.UpdateVilla(villaUpdateDto, existingVilla);
                existingVilla.FechaActualizacion = DateTime.Now;
                // Guardamos los cambios
                await _context.SaveChangesAsync();
                // Mapeamos a DTO para retornar
                var villaDto = _mapper.ToDto(existingVilla);
                // Crear response
                var response = ApiResponse<VillaResponseDto>.Ok("Villa actualizada exitosamente", villaDto);
                // Retornamos la villa actualizada con status 200 OK
                return Ok(response);
            }
            catch(Exception ex)
            {
                // Crear response
                var response = ApiResponse<object>.Error(ex, "Error al actualizar la villa");
                // Retornar error si algo sale mal
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                // Verificar si la villa existe
                var villa = await _context.Villas.FindAsync(id);
                if (villa == null)
                    return NotFound(ApiResponse<object>.NotFound($"La villa con ID {id} no existe"));
                
                // Eliminar la villa
                _context.Villas.Remove(villa);
                await _context.SaveChangesAsync();
                
                // Retornar status 204 No Content
                return Ok(ApiResponse<object>.NoContent("Villa eliminada exitosamente"));
            }
            catch(Exception ex)
            {
                // Crear response
                var response = ApiResponse<object>.Error(ex, "Error al eliminar la villa");
                // Retornar error si algo sale mal
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }



    }
}