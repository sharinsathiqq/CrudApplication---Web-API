using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudApplication.Database;
using CrudApplication.Models;

namespace CrudApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDBContext _studentDbContext;
        private readonly ILogger<StudentController> _logger;

        public StudentController(StudentDBContext studentDbContext, ILogger<StudentController> logger)
        {
            _studentDbContext = studentDbContext;
            _logger = logger;
        }

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _studentDbContext.Students.ToListAsync();
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(Guid id)
        {
            var student = await _studentDbContext.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // POST: api/Student
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent([FromForm] Student student, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                if (await UploadProfilePhoto(file) is OkObjectResult result)
                {
                    student.ProfilePhotoFile = result.Value.ToString();
                }
            }

            student.Id = Guid.NewGuid();
            _studentDbContext.Students.Add(student);
            await _studentDbContext.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        [HttpPost("Image")]
        public async Task<IActionResult> UploadProfilePhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded.");
                return BadRequest("No file uploaded.");
            }

            var basePath = Directory.GetCurrentDirectory();
            var filesFolderPath = Path.Combine(basePath, "Assets", "uploads");
            if (!Directory.Exists(filesFolderPath))
            {
                Directory.CreateDirectory(filesFolderPath);
                _logger.LogInformation($"Created directory: {filesFolderPath}");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(filesFolderPath, fileName);

            _logger.LogInformation($"Uploading file to: {filePath}");

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Ok(fileName);
        }

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(Guid id, [FromForm] Student student, IFormFile? file)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            var existingStudent = await _studentDbContext.Students.FindAsync(id);
            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.name = student.name;
            existingStudent.emailID = student.emailID;
            existingStudent.phoneNumber = student.phoneNumber;
            existingStudent.dob = student.dob;
            existingStudent.departmentId = student.departmentId;
            existingStudent.Address = student.Address; // Update the address field

            if (file != null && file.Length > 0)
            {
                var result = await UploadProfilePhoto(file) as OkObjectResult;
                if (result != null)
                {
                    existingStudent.ProfilePhotoFile = result.Value.ToString();
                }
            }

            _studentDbContext.Entry(existingStudent).State = EntityState.Modified;

            try
            {
                await _studentDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _studentDbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(student.ProfilePhotoFile))
            {
                // Optionally handle file deletion logic here
            }

            _studentDbContext.Students.Remove(student);
            await _studentDbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(Guid id)
        {
            return _studentDbContext.Students.Any(e => e.Id == id);
        }
    }
}
