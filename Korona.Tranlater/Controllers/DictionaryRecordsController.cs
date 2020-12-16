using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Korona.Tranlater.Data;
using Korona.Tranlater.Models;

namespace Korona.Tranlater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryRecordsController : ControllerBase
    {
        private readonly KoronaDbContext _context;

        public DictionaryRecordsController(KoronaDbContext context)
        {
            _context = context;
        }

        // GET: api/DictionaryRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DictionaryRecord>>> GetDictionaryRecords()
        {
            return await _context.DictionaryRecords.ToListAsync();
        }
        
        // GET: api/DictionaryRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DictionaryRecord>> GetDictionaryRecord(int id)
        {
            var dictionaryRecord = await _context.DictionaryRecords.FindAsync(id);

            if (dictionaryRecord == null)
            {
                return NotFound();
            }

            return dictionaryRecord;
        }

        // PUT: api/DictionaryRecords/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDictionaryRecord(int id, DictionaryRecord dictionaryRecord)
        {
            if (id != dictionaryRecord.Id)
            {
                return BadRequest();
            }

            _context.Entry(dictionaryRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DictionaryRecordExists(id))
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

        // POST: api/DictionaryRecords
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DictionaryRecord>> PostDictionaryRecord(DictionaryRecord dictionaryRecord)
        {
            _context.DictionaryRecords.Add(dictionaryRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDictionaryRecord", new { id = dictionaryRecord.Id }, dictionaryRecord);
        }

        // DELETE: api/DictionaryRecords/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DictionaryRecord>> DeleteDictionaryRecord(int id)
        {
            var dictionaryRecord = await _context.DictionaryRecords.FindAsync(id);
            if (dictionaryRecord == null)
            {
                return NotFound();
            }

            _context.DictionaryRecords.Remove(dictionaryRecord);
            await _context.SaveChangesAsync();

            return dictionaryRecord;
        }

        private bool DictionaryRecordExists(int id)
        {
            return _context.DictionaryRecords.Any(e => e.Id == id);
        }
    }
}
