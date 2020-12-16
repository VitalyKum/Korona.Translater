using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Korona.Tranlater.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Korona.Tranlater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileHandlersController : ControllerBase
    {
        private readonly KoronaDbContext _context;
        private readonly IConfiguration _configuration;
        public FileHandlersController(KoronaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> GetHandledFile(int processSchemaId, IFormFile formfile)
        {
            //var schema = await _context.FileHandleSchemas.FindAsync(processSchemaId);

            //if (schema == null ||
            //    formfile == null || 
            //    schema?.InputContentType != formfile.ContentType)
            //    return BadRequest();

            //using (var stream = new MemoryStream())
            //{
            //    await formfile.CopyToAsync(stream);

            //    // Upload the file if less than 5 MB
                
            //    if (stream.Length < 5000000)
            //    {
            //        //var handler = new FileHandler(schema, stream.ToArray());
                    
            //        //var file = handler.Handle();

            //        //if (file == null || file.Length == 0)
            //        //    return StatusCode((int)HttpStatusCode.InternalServerError);
            //        //else
            //            //return File(file, schema.OutputContentType);
            //    }
            //    else
            //    {
            //        throw new InvalidDataException("File size over 5Mb");
            //    }
            //}

            return Ok();
        }
    }
}