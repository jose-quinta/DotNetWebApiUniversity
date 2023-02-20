using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class FacultyController : ControllerBase {
        private readonly ApplicationDbContext _context;
        public FacultyController(ApplicationDbContext context) => _context = context;
        [HttpGet]
        public async Task<ActionResult<List<Faculty>>> Get() {
            var response = await _context.Faculties.ToListAsync();

            if (response == null)
                return BadRequest($"There are not faculties yet!!");

            foreach (var item in response) {
                var schools = await _context.Schools.Where(x => x.FacultyId == item.Id).ToListAsync();
                foreach (var school in schools) {
                    school.Faculty = null!;

                    var careers = await _context.Careers.Where(x => x.SchoolId == school.Id).ToListAsync();
                    foreach (var career in careers) {
                        career.School = null!;
                    }

                    school.Careers = careers;
                }

                item.Schools = schools;
            }

            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Faculty>> GetById(int id) {
            var response = await _context.Faculties.FindAsync(id);

            if (response == null)
                return BadRequest($"Faculty does not exist or is {response}");

            var schools = await _context.Schools.Where(x => x.FacultyId == response.Id).ToListAsync();
            foreach (var school in schools) {
                school.Faculty = null!;

                var careers = await _context.Careers.Where(x => x.SchoolId == school.Id).ToListAsync();
                foreach (var career in careers) {
                    career.School = null!;
                }

                school.Careers = careers;
            }

            response.Schools = schools;

            return Ok(response);
        }
        [HttpGet("ByCode/{code}")]
        public async Task<ActionResult<SchoolsName>> GetNamesByCode(string code) {
            var _response = await _context.Faculties.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (_response == null)
                return BadRequest($"Faculty does not exist or is {_response}");

            var _schools = await _context.Schools.Where(x => x.FacultyId == _response!.Id).ToListAsync();

            var response = new SchoolsName() {
                Name = _response!.Name
            };

            foreach (var item in _schools) {
                response.Names.Add(item.Name);
            }

            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<List<Faculty>>> Post(FacultyDto request) {
            if (request == null)
                return BadRequest($"Faculty data is empty or is {request}");

            var _request = new Faculty() {
                Name = request.Name,
                Code = request.Code
            };

            await _context.AddAsync(_request);
            await _context.SaveChangesAsync();

            var response = await _context.Faculties.ToListAsync();

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Faculty>> Put(int id, FacultyDto request) {
            if (request ==  null)
                return BadRequest($"Faculty data is empty or is {request}");

            var response = await _context.Faculties.FindAsync(id);

            if (response == null)
                return BadRequest($"Faculty does not exist or is {response}");

            response.Name = request.Name;
            response.Code = request.Code;

            await _context.SaveChangesAsync();

            var schools = await _context.Schools.Where(x => x.FacultyId == response.Id).ToListAsync();
            foreach (var school in schools) {
                school.Faculty = null!;

                var careers = await _context.Careers.Where(x => x.SchoolId == school.Id).ToListAsync();
                foreach (var career in careers) {
                    career.School = null!;
                }

                school.Careers = careers;
            }

            response.Schools = schools;

            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Faculty>> Delete(int id) {
            var response = await _context.Faculties.FindAsync(id);

            if (response == null)
                return BadRequest($"Faculty does not exist or is {response}");

            _context.Remove(response);
            await _context.SaveChangesAsync();

            var schools = await _context.Schools.Where(x => x.FacultyId == response.Id).ToListAsync();
            foreach (var school in schools) {
                school.Faculty = null!;

                var careers = await _context.Careers.Where(x => x.SchoolId == school.Id).ToListAsync();
                foreach (var career in careers) {
                    career.School = null!;
                }

                school.Careers = careers;
            }

            response.Schools = schools;

            return Ok(response);
        }
        [HttpDelete("ByCode/{code}")]
        public async Task<ActionResult<Faculty>> DeleteByCode(string code) {
            var response = await _context.Faculties.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (response == null)
                return BadRequest($"Faculty does not exist or is {response}");

            _context.Remove(response);
            await _context.SaveChangesAsync();

            var schools = await _context.Schools.Where(x => x.FacultyId == response.Id).ToListAsync();
            foreach (var school in schools) {
                school.Faculty = null!;

                var careers = await _context.Careers.Where(x => x.SchoolId == school.Id).ToListAsync();
                foreach (var career in careers) {
                    career.School = null!;
                }

                school.Careers = careers;
            }

            response.Schools = schools;

            return Ok(response);
        }
    }
}