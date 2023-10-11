using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi2.Models;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Data.Sqlite;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System;
using System.IO;
using DinkToPdf;
using System.Data.SQLite;
using System.Net;
using System.Net.Http.Headers;

namespace TodoApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculeController : ControllerBase
    {
        private readonly VehiculeContext _context;

        public VehiculeController(VehiculeContext context)
        {
            _context = context;
        }


        [HttpPost("/download/{filename}")]
        [SwaggerOperation(OperationId = nameof(DownloadAsync), Tags = new[] {""})]
        [SwaggerResponse(StatusCodes.Status200OK, nameof(StatusCodes.Status200OK), typeof(Stream))]
        public async Task<FileStreamResult> DownloadAsync(string filename)
        //public HttpResponseMessage 
        {

            // var todoItems = await _context.Vehicule.ToListAsync();

            
            string htmlTemplate = ReadFileContent(filename);
            var sqlQueries = ExtractSqlQueries(htmlTemplate);

            // Print the extracted SQL queries
            Console.WriteLine("Extracted SQL Queries:");
            string[] results = new string[sqlQueries.Length];
            int i = 0;
            foreach (var query in sqlQueries)
            {
                results[i] = ExecuteSqlQuery(query);
                htmlTemplate = ReplacePlaceholders(htmlTemplate, query, results[i]);

                i++;
            }


            string pdfFilePath = "output.pdf";
            var converter = new BasicConverter(new PdfTools());

                    var doc = new HtmlToPdfDocument
                    {
                        GlobalSettings = {
                        ColorMode = DinkToPdf.ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = DinkToPdf.PaperKind.A4,
                    },
                        Objects = {
                new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = htmlTemplate,
                }
            }
                    };

                    var pdfBytes = converter.Convert(doc);
           
            return new FileStreamResult(new MemoryStream(pdfBytes), "application/octet-stream");
                
            
        }
        static void GeneratePdfFromHtml(string html, string pdfFilePath)
        {
            var converter = new BasicConverter(new PdfTools());

            var doc = new HtmlToPdfDocument
            {
                GlobalSettings = {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
            },
                Objects = {
                new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = html,
                }
            }
            };

            var pdfBytes = converter.Convert(doc);
            System.IO.File.WriteAllBytes(pdfFilePath, pdfBytes);
        }

        static string[] ExtractSqlQueries(string html)
        {
            // Define a regular expression pattern for extracting SQL queries
            string pattern = @"\{\{SQL:\s*(.*?)\}\}";

            // Use Regex to match the pattern in the HTML
            var matches = Regex.Matches(html, pattern);

            // Extract and return the matched SQL queries
            var queries = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                queries[i] = matches[i].Groups[1].Value;
            }

            return queries;
            // Replace SQL query placeholders with results

        }

        static string ReadFileContent(string filePath)
        {
            try
            {
                return System.IO.File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return string.Empty;
            }
        }

        static string ExecuteSqlQuery(string sqlQuery)
        {
            // Replace this connection string with your database connection string
            string databasePath = "d:\\mini_projet\\database.db";
            string connectionString = $"Data Source={databasePath};";


            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sqlQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    return result?.ToString() ?? string.Empty;
                }
            }
        }

        static string ReplacePlaceholders(string template, params string[] keyValues)
        {
            if (keyValues.Length % 2 != 0)
            {
                throw new ArgumentException("Key-value pairs are not balanced.");
            }

            for (int i = 0; i < keyValues.Length; i += 2)
            {
                string key = keyValues[i];
                string value = keyValues[i + 1];

                // Replace placeholders in the template with actual values
                template = template.Replace("{{SQL: " + key + "}}", value);
            }

            return template;
        }
        // GET: api/Vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicule>>> GetVehicules()
        {
          if (_context.Vehicule == null)
          {
              return NotFound();
          }
            return await _context.Vehicule.ToListAsync();
        }

        // GET: api/Vehicles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicule>> GetVehicule(long id)
        {
          if (_context.Vehicule == null)
          {
              return NotFound();
          }
            var todoItem = await _context.Vehicule.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicule(long id, Vehicule todoItem)
        {
            if (id != todoItem.vehicule_id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiculeExists(id))
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

        // POST: api/Vehicles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vehicule>> PostVehicule(Vehicule todoItem)
        {
          if (_context.Vehicule == null)
          {
              return Problem("Entity set 'TodoContext.Vehicules'  is null.");
          }
            _context.Vehicule.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.vehicule_id }, todoItem);
        }

        // DELETE: api/Vehicles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicule(long id)
        {
            if (_context.Vehicule == null)
            {
                return NotFound();
            }
            var todoItem = await _context.Vehicule.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Vehicule.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehiculeExists(long id)
        {
            return (_context.Vehicule?.Any(e => e.vehicule_id == id)).GetValueOrDefault();
        }
    }
}
