using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Korona.Tranlater.Data;
using Korona.Tranlater.Models;

namespace Korona.Tranlater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslateSсhemaController : ControllerBase
    {
        private readonly KoronaDbContext _context;

        public TranslateSсhemaController(KoronaDbContext context)
        {
            _context = context;
        }

        // GET: api/TranslateSсhema
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TranslateSсhema>>> GetTranslateSchemes()
        {
            return await _context.TranslateSchemes.ToListAsync();
        }

        // GET: api/TranslateSсhema/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TranslateSсhema>> GetTranslateSсhema(int id)
        {
            var translateSсhema = await _context.TranslateSchemes.FindAsync(id);

            if (translateSсhema == null)
            {
                return NotFound();
            }

            return translateSсhema;
        }

        // PUT: api/TranslateSсhema/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTranslateSсhema(int id, TranslateSсhema translateSсhema)
        {
            if (id != translateSсhema.Id)
            {
                return BadRequest();
            }

            _context.Entry(translateSсhema).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TranslateSсhemaExists(id))
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

        // POST: api/TranslateSсhema
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TranslateSсhema>> PostTranslateSсhema(TranslateSсhema translateSсhema)
        {
            _context.TranslateSchemes.Add(translateSсhema);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTranslateSсhema", new { id = translateSсhema.Id }, translateSсhema);
        }

        // DELETE: api/TranslateSсhema/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TranslateSсhema>> DeleteTranslateSсhema(int id)
        {
            var translateSсhema = await _context.TranslateSchemes.FindAsync(id);
            if (translateSсhema == null)
            {
                return NotFound();
            }

            _context.TranslateSchemes.Remove(translateSсhema);
            await _context.SaveChangesAsync();

            return translateSсhema;
        }

        private bool TranslateSсhemaExists(int id)
        {
            return _context.TranslateSchemes.Any(e => e.Id == id);
        }
    }
}
