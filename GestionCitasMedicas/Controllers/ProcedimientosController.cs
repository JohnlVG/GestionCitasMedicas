using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static GestionCitasMedicas.Entidades;

namespace GestionCitasMedicas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcedimientosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProcedimientosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Procedimientos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Procedimiento>>> GetProcedimientos()
        {
            return await _context.Procedimientos.ToListAsync();
        }

        // GET: api/Procedimientos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Procedimiento>> GetProcedimiento(int id)
        {
            var procedimiento = await _context.Procedimientos.FindAsync(id);

            if (procedimiento == null)
            {
                return NotFound();
            }

            return procedimiento;
        }

        [HttpPost]
        public async Task<IActionResult> PostProcedimiento([FromBody] Procedimiento procedimiento)
        {
            // Validación de Cita y Procedimiento
            if (procedimiento.IdCita <= 0)
            {
                return BadRequest("El IdCita debe ser válido.");
            }

            // Obtener la Cita relacionada
            var cita = await _context.Citas.FindAsync(procedimiento.IdCita);
            if (cita == null)
            {
                return NotFound("La Cita no fue encontrada.");
            }

            // Asignar la Cita al Procedimiento
            procedimiento.Cita = cita;

            // Guardar el Procedimiento
            _context.Procedimientos.Add(procedimiento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProcedimiento), new { id = procedimiento.IdProcedimiento }, procedimiento);
        }




        // PUT: api/Procedimientos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcedimiento(int id, Procedimiento procedimiento)
        {
            if (id != procedimiento.IdProcedimiento)
            {
                return BadRequest();
            }

            _context.Entry(procedimiento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcedimientoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Procedimientos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedimiento(int id)
        {
            var procedimiento = await _context.Procedimientos.FindAsync(id);
            if (procedimiento == null)
            {
                return NotFound();
            }

            _context.Procedimientos.Remove(procedimiento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProcedimientoExists(int id)
        {
            return _context.Procedimientos.Any(e => e.IdProcedimiento == id);
        }
    }
}
