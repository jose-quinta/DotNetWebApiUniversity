using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class CareerController : ControllerBase {
        private readonly ApplicationDbContext _context;
        public CareerController(ApplicationDbContext context) => _context = context;
        [HttpGet]
        public async Task<ActionResult<List<Career>>> Get() {
            var response = await _context.Careers.ToListAsync();

            if (response == null)
                return BadRequest($"There are not careers yet!!!");

            foreach (var item in response) {
                var school = await _context.Schools.FindAsync(item.SchoolId);
                school!.Careers = new List<Career>();

                var faculty = await _context.Faculties.FindAsync(school.FacultyId);
                faculty!.Schools = new List<School>();
                school.Faculty = faculty;

                item.School = school;
            }

            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Career>> GetById(int id) {
            var response = await _context.Careers.FindAsync(id);

            if (response ==  null)
                return BadRequest($"Career does not exist or is {response}");

            var school = await _context.Schools.FindAsync(response.SchoolId);
            school!.Careers = new List<Career>();

            var faculty = await _context.Faculties.FindAsync(school.FacultyId);
            faculty!.Schools = new List<School>();
            school.Faculty = faculty;

            response.School = school;

            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<List<Career>>> Post(CareerDto request) {
            if (request == null)
                return BadRequest($"Career data is empty or is {request}");

            var _request = new Career(){
                Name = request.Name,
                Code = request.Code,
                SchoolId = request.SchoolId
            };

            await _context.AddAsync(_request);
            await _context.SaveChangesAsync();

            var response = await _context.Careers.ToListAsync();

            foreach (var item in response) {
                var school = await _context.Schools.FindAsync(item.SchoolId);
                school!.Careers = new List<Career>();

                var faculty = await _context.Faculties.FindAsync(school.FacultyId);
                faculty!.Schools = new List<School>();
                school.Faculty = faculty;

                item.School = school;
            }

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Career>> GetById(int id, CareerDto request) {
            if (request == null)
                return BadRequest($"Career data is empty or is {request}");

            var response = await _context.Careers.FindAsync(id);

            if (response == null)
                return BadRequest($"Career does not exist or is {response}");

            response.Name = request.Name;
            response.Code = request.Code;
            response.SchoolId = request.SchoolId;

            await _context.SaveChangesAsync();

            var school = await _context.Schools.FindAsync(response.SchoolId);
            school!.Careers = new List<Career>();

            var faculty = await _context.Faculties.FindAsync(school.FacultyId);
            faculty!.Schools = new List<School>();
            school.Faculty = faculty;

            response.School = school;

            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Career>> Delete(int id) {
            var response = await _context.Careers.FindAsync(id);

            if (response == null)
                return BadRequest($"Career does not exist or is {response}");

            _context.Remove(response);
            await _context.SaveChangesAsync();

            var school = await _context.Schools.FindAsync(response.SchoolId);
            school!.Careers = new List<Career>();

            var faculty = await _context.Faculties.FindAsync(school.FacultyId);
            faculty!.Schools = new List<School>();
            school.Faculty = faculty;

            response.School = school;

            return Ok(response);
        }
        [HttpDelete("ByCode/{code}")]
        public async Task<ActionResult<Career>> DeleteByCode(string code) {
            var response = await _context.Careers.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (response == null)
                return BadRequest($"Career does not exist or is {response}");

            _context.Remove(response);
            await _context.SaveChangesAsync();

            var school = await _context.Schools.FindAsync(response.SchoolId);
            school!.Careers = new List<Career>();

            var faculty = await _context.Faculties.FindAsync(school.FacultyId);
            faculty!.Schools = new List<School>();
            school.Faculty = faculty;

            response.School = school;

            return Ok(response);
        }
    }
}