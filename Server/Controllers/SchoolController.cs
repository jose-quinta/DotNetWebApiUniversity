using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolController : ControllerBase {
        private readonly ApplicationDbContext _context;
        public SchoolController(ApplicationDbContext context) => _context = context;
        [HttpGet]
        public async Task<ActionResult<List<School>>> Get() {
            var response = await _context.Schools.ToListAsync();

            if (response == null)
                return BadRequest($"There are not schools yet!!!");

            foreach (var item in response) {
                var faculty = await _context.Faculties.FindAsync(item.FacultyId);
                faculty!.Schools = new List<School>();

                var careers = await _context.Careers.Where(x => x.SchoolId == item.Id).ToListAsync();
                foreach (var career in careers) {
                    career.School = null!;
                }

                item.Faculty = faculty;
                item.Careers = careers;
            }

            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<School>> GetById(int id) {
            var response = await _context.Schools.FindAsync(id);

            if (response == null)
                return BadRequest($"School does not exist or is {response}");

            var faculty = await _context.Faculties.FindAsync(response.FacultyId);
            faculty!.Schools = new List<School>();

            var careers = await _context.Careers.Where(x => x.SchoolId == response.Id).ToListAsync();
            foreach (var item in careers) {
                item.School = null!;
            }

            response.Faculty = faculty;
            response.Careers = careers;

            return Ok(response);
        }
        [HttpGet("ByCode/{code}")]
        public async Task<ActionResult<CareersName>> GetNamesByCode(string code) {
            var _response = await _context.Schools.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (_response == null)
                return BadRequest($"School does not exist or is {_response}");

            var _careers = await _context.Careers.Where(x => x.SchoolId == _response!.Id).ToListAsync();

            var response = new CareersName() {
                Name = _response!.Name
            };

            foreach (var item in _careers) {
                response.Names.Add(item.Name);
            }

            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<List<School>>> Post(SchoolDto request) {
            if (request ==  null)
                return BadRequest($"School data is empty or is {request}");

            var _request = new School() {
                Name = request.Name,
                Code = request.Code,
                FacultyId = request.FacultyId
            };

            await _context.AddAsync(_request);
            await _context.SaveChangesAsync();

            var response = await _context.Schools.ToListAsync();

            foreach (var item in response) {
                var faculty = await _context.Faculties.FindAsync(item.FacultyId);
                faculty!.Schools = new List<School>();

                var careers = await _context.Careers.Where(x => x.SchoolId == item.Id).ToListAsync();
                foreach (var career in careers) {
                    career.School = null!;
                }

                item.Faculty = faculty;
                item.Careers = careers;
            }

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<School>> Put(int id, SchoolDto request) {
            if (request == null)
                return BadRequest($"School data is empty or is {request}");

            var response = await _context.Schools.FindAsync(id);

            response!.Name = request.Name;
            response.Code = request.Code;
            response.FacultyId = request.FacultyId;

            await _context.SaveChangesAsync();

            var faculty = await _context.Faculties.FindAsync(response.FacultyId);
            faculty!.Schools = new List<School>();

            var careers = await _context.Careers.Where(x => x.SchoolId == response.Id).ToListAsync();
            foreach (var item in careers) {
                item.School = null!;
            }

            response.Faculty = faculty;
            response.Careers = careers;

            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<School>> Delete(int id) {
            var response = await _context.Schools.FindAsync(id);

            if (response == null)
                return BadRequest($"School does not exist or is {response}");

            _context.Remove(response);
            await _context.SaveChangesAsync();

            var faculty = await _context.Faculties.FindAsync(response.FacultyId);
            faculty!.Schools = new List<School>();

            var careers = await _context.Careers.Where(x => x.SchoolId == response.Id).ToListAsync();
            foreach (var item in careers) {
                item.School = null!;
            }

            response.Faculty = faculty;
            response.Careers = careers;

            return Ok(response);
        }
        [HttpDelete("ByCode/{code}")]
        public async Task<ActionResult<School>> DeleteByCode(string code) {
            var response = await _context.Schools.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (response == null)
                return BadRequest($"School does not exist or is {response}");

            _context.Remove(response);
            await _context.SaveChangesAsync();

            var faculty = await _context.Faculties.FindAsync(response.FacultyId);
            faculty!.Schools = new List<School>();

            var careers = await _context.Careers.Where(x => x.SchoolId == response.Id).ToListAsync();
            foreach (var item in careers) {
                item.School = null!;
            }

            response.Faculty = faculty;
            response.Careers = careers;

            return Ok(response);
        }
    }
}